﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {


    public float pulseDistance = 3.0f;
    public float pulseDurationSetting = 5.0f;
    public float pulseDisplaySize = 4.0f;
    public float pulseStrength = 4.0f;
    public float affectLimit = 2.5f;    
    public float drainSpeed = 1.0f;
    public bool gameComplete = false;
    public int score = 0;
    public int tapScoreEffect = -50;
    public LevelScoring scoring;
    MagneticPulse activePulse;
    [HideInInspector] public int tapCount = 0;
    [HideInInspector] public float seconds = 0.0f;
    float maxDistanceAffect = 0.75f;
    float maxPulseAffect = 1.0f;
    float pulseDistanceThreshold = 0.0f;

    UI userInterface;
    [HideInInspector] public List<MagneticPulse> preconfiguredPulses = new List<MagneticPulse>();
    [HideInInspector] public List<MagneticPulse> attractionPulses = new List<MagneticPulse>();
    [HideInInspector] public List<MagneticPulse> repulsionPulses = new List<MagneticPulse>();
    public GameObject pulseImage;
    public AudioSource attractPulseAudio;
    public GameObject repulsionImage;
    int pulseIndex = 1;
    List<int> indexList = new List<int>();
    MagneticBall[] magneticObjects;

    public void SetupManager(LevelScoring scoreConfiguration)
    {
        for (int i = 0; i < attractionPulses.Count; i++){
            attractionPulses[i].pulseStrength = 0.0f;
            Destroy(attractionPulses[i].prefab);
        }
        scoring = scoreConfiguration;
        LoadMagneticObjects();
        LoadPreconfiguredPulses();
        tapCount = 0;
        seconds = 0.0f;
        score = 0;
        userInterface.SetTapCount(tapCount);
        userInterface.SetScore(score);
        userInterface.SetTime(seconds);
    }
    private List<int> FullIndexList()
    {
        List<int> returnList = new List<int>();
        foreach (MagneticBall obj in magneticObjects)
        {
            returnList.Add(obj.index);
        }
        return returnList;
    }

    private void Start()
    {
        userInterface = FindObjectOfType<UI>();
    }
    void LoadMagneticObjects()
    {        
        magneticObjects = FindObjectsOfType<MagneticBall>();
        int index = 0;
        foreach (MagneticBall mag in magneticObjects)
        {
            mag.index = index;
            index++;
        }
    }
    void LoadPreconfiguredPulses()
    {
        preconfiguredPulses = new List<MagneticPulse>();
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        foreach (Attractor attract in attractors)
        {
            preconfiguredPulses.Add(CreatePulse(attract.transform.position, attract.strength, 1.0f));
        }
    }
    void UpdatePulses()
    {
        List<MagneticPulse> nextPulseList = new List<MagneticPulse>();
        for (int i = 0; i < attractionPulses.Count; i++)
        {
            MagneticPulse p = attractionPulses[i];
            float dissapateFactor = p.dissapates ? 1.0f : 0.0f;
            float pDuration = p.pulseRemaining - (Time.deltaTime * drainSpeed) * dissapateFactor;
            pDuration = Mathf.Max(pDuration, 0.0f);
            p.pulseRemaining = pDuration;
            if (p.pulseRemaining > 0)
            {
                nextPulseList.Add(p);
                float scaleSize = pulseDisplaySize * p.pulseRemaining/p.pulseDuration;
                if (p.pulseStrength < 0.0f)
                {
                        scaleSize = pulseDisplaySize * (p.pulseDuration / p.pulseRemaining * 2.0f);
                        scaleSize = Mathf.Min(scaleSize, 8.0f);
                }
                p.prefab.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            }
            else
            {
                Destroy(p.prefab);
                Destroy(p);
            }
        }
        attractionPulses = nextPulseList;

    }
    void CheckForRemaining()
    {
        foreach (MagneticBall obj in magneticObjects)
        {
            if (obj != null)
            {
                return;
            }
        }
        gameComplete = true;
        RemoveActivePulses();
        userInterface.ShowGameCompleteMenu();
    }
    public void AddScore(int scoreAdjustment)
    {
        score += scoreAdjustment;
        score = Mathf.Max(score, 0);
        userInterface.SetScore(score);
    }
    public void AddRepulsion(Vector3 pos, int[] affectedIndeces, float repulsionStrength = 0.0f)
    {
        if (repulsionStrength == 0.0f) repulsionStrength = -pulseStrength;
        //repulsionStrength = (-pulseStrength) / 2.5f;
        MagneticPulse p = CreatePulse(position: pos, strength: repulsionStrength, duration: 0.75f, prefab: repulsionImage);
        p.specificIndeces = affectedIndeces.ToList();
        attractionPulses.Add(p);
    }
    public void RemoveFromCurrentAttractionPulses(int index)
    {
        foreach (MagneticPulse p in attractionPulses)
        {
            if (p.pulseStrength > 0.0f){
                if (p.specificIndeces.Contains(index)){
                    p.specificIndeces.Remove(index);
                }
            }
        }
    }

    MagneticPulse CreatePulse(Vector3 position, float strength, float duration)
    {
        MagneticPulse pulse =  ScriptableObject.CreateInstance<MagneticPulse>();
        pulse.index = pulseIndex;
        pulseIndex++;
        pulse.pulseStrength = strength;
        pulse.pulseDuration = duration;
        pulse.pulseRemaining = pulse.pulseDuration;
        pulse.pulseLocation = position;
        pulse.hasBeenReached = false;
        return pulse;
    }

    MagneticPulse CreatePulse(Vector3 position, float strength, float duration, GameObject prefab)
    {
        MagneticPulse pulse = CreatePulse(position, strength, duration);
        pulse.prefab = Instantiate(prefab, pulse.pulseLocation, Quaternion.Euler(0.0f, 0.0f, 0.0f),transform);
        pulse.prefab.GetComponent<ParticleSystem>().Simulate(2.0f);
        pulse.prefab.GetComponent<ParticleSystem>().Play();

        return pulse;
    }
    void AddPulseAtPosition(Vector3 pos)
    {
        if (Time.timeScale > 0){
            MagneticPulse p = CreatePulse(position: pos, strength: pulseStrength, duration: pulseDurationSetting, prefab: pulseImage);
            p.specificIndeces = FullIndexList();
            if (p.pulseStrength > 0.0f)
            {
                p.dissapates = false;
                activePulse = p;
            }
            attractionPulses.Add(p);
        }

    }
    public void RemoveActivePulses()
    {
        foreach (MagneticPulse p in attractionPulses){
            p.Dissapate();
        }
    }
    void SetPulseToInputPos()
    {
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 loc2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 pulseLocation = new Vector3(loc.x, loc.y, 1.0f);
        if (activePulse){
            activePulse.prefab.transform.position = pulseLocation;
            activePulse.pulseLocation = pulseLocation;
//            obj.transform.position = pulseLocation;
        }
    }

    void AddPulse()
    {
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 loc2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 pulseLocation = new Vector3(loc.x, loc.y, 1.0f);
        AddPulseAtPosition(pulseLocation);
    }
    Vector3 AffectOnBallFromUserTaps(MagneticBall ball)
    {
        Vector3 totalForce = Vector3.zero;

        for (int i = 0; i < attractionPulses.Count; i++)
        {
            MagneticPulse p = attractionPulses[i];
            if (p.specificIndeces.Contains(ball.index))
            {
                Vector3 vecA = new Vector3(ball.transform.position.x, ball.transform.position.y, 0.0f);
                Vector3 vecB = new Vector3(p.pulseLocation.x, p.pulseLocation.y, 0.0f);

                float distanceApart = Mathf.Abs(Vector3.Distance(vecA, vecB));
                //if (distanceApart > pulseDistanceThreshold)
                //{
                    float dist = 1.0f / distanceApart;
                    dist = Mathf.Min(dist, maxDistanceAffect);
                    dist = 1.0f;
                    float pulseAffect = p.pulseRemaining / p.pulseDuration;
                    //pulseAffect = 1.0f;
                    totalForce += (new Vector3(ball.transform.position.x, ball  .transform.position.y, p.pulseLocation.z) - p.pulseLocation).normalized * p.pulseStrength * pulseAffect * dist;

                //}
            }
        }
        return totalForce;
    }
    Vector3 AffectOnBallFromAttractors(MagneticBall ball)
    {
        Vector3 totalForce = Vector3.zero;
        foreach (MagneticPulse p in preconfiguredPulses)
        {
            Vector3 vecA = new Vector3(ball.transform.position.x, ball.transform.position.y, 0.0f);
            Vector3 vecB = new Vector3(p.pulseLocation.x, p.pulseLocation.y, 0.0f);

            float distanceApart = Mathf.Abs(Vector3.Distance(ball.transform.position, p.pulseLocation));
            if (distanceApart > pulseDistanceThreshold)
            {
                float dist = 1.0f / distanceApart;
                dist = Mathf.Min(dist, maxDistanceAffect);
                totalForce += (new Vector3(ball.transform.position.x, ball.transform.position.y, p.pulseLocation.z) - p.pulseLocation).normalized * p.pulseStrength * dist;
            }
        }
        return totalForce;
    }
    Vector3 AffectOnBallFromNearbyMagneticBalls(MagneticBall ball)
    {
        if (ball.lockedInOrbit) return Vector2.zero;

        float nearbyBallAffectThreshld = 1.5f;
        Vector3 totalAffect = new Vector3(0.0f, 0.0f, 0.0f);
        foreach (MagneticBall otherBall in magneticObjects)
        {
            if (otherBall == ball || otherBall == null || otherBall.type == ball.type) continue;
            float distanceApart = Mathf.Abs(Vector3.Distance(otherBall.transform.position, ball.transform.position));
            if (distanceApart <= nearbyBallAffectThreshld){
                //Debug.Log("Index: " + ball.index + " is distance: " + distanceApart + " from index: " + otherBall.index);
                float dist = 1.0f / distanceApart;
                dist = Mathf.Min(dist, maxDistanceAffect);
                Vector3 affect = (ball.transform.position - otherBall.transform.position).normalized * pulseStrength/5.0f * dist;
                //otherBall.ActivateShields();
                //ball.ActivateShields();
                totalAffect += new Vector3(affect.x, affect.y, 0.0f);
            }
        }
        return totalAffect;        
    }
    public Vector3 TotalForceOnBall(MagneticBall ball)
    {
        Vector3 totalForce = Vector3.zero;
        totalForce += AffectOnBallFromUserTaps(ball);
        if (!ball.lockedInOrbit)
        {
            totalForce += AffectOnBallFromAttractors(ball);
            totalForce += AffectOnBallFromNearbyMagneticBalls(ball);
        }

        return ClippedPulse(totalForce);
    }
    Vector3 ClippedPulse(Vector3 originalPulse)
    {
        float xVal = Mathf.Max(-originalPulse.x, -maxPulseAffect);
        float yVal = Mathf.Min(originalPulse.y, maxPulseAffect);
        return new Vector3(xVal, yVal, 0.0f);
    }

    //public Vector2 AffectOnPosition(Vector2 position, int objectIndex, bool isInOrbit = false)
    //{

    //    Vector3 pulsePower = new Vector2(0.0f, 0.0f);
    //    List<MagneticPulse> pulsesReached = new List<MagneticPulse>();
    //    for (int i = 0; i < attractionPulses.Count; i++)
    //    {
    //        MagneticPulse p = attractionPulses[i];
    //        if (p.specificIndeces.Contains(objectIndex))
    //        {
    //            Vector3 vecA = new Vector3(position.x, position.y, 0.0f);
    //            Vector3 vecB = new Vector3(p.pulseLocation.x,p.pulseLocation.y, 0.0f);

    //            float distanceApart = Mathf.Abs(Vector3.Distance(vecA, vecB));
    //            if (distanceApart > pulseDistanceThreshold)
    //            {
    //                float dist = 1.0f / distanceApart;
    //                dist = Mathf.Min(dist, maxDistanceAffect);
    //                float pulseAffect = p.pulseRemaining / p.pulseDuration;
    //                pulseAffect = 1.0f;
    //                pulsePower += (new Vector3(position.x, position.y, p.pulseLocation.z) - p.pulseLocation).normalized * p.pulseStrength * pulseAffect * dist;

    //            }
    //            else{
    //                //if (p.specificIndeces.Contains(objectIndex))
    //                //{
    //                //    p.specificIndeces.Remove(objectIndex);
    //                //}
    //                //RemoveFromCurrentAttractionPulses(objectIndex);

    //            }
    //        }
    //    }
    //    if (!isInOrbit)
    //    {
    //        foreach (MagneticPulse p in preconfiguredPulses)
    //        {
    //            Vector3 vecA = new Vector3(position.x, position.y, 0.0f);
    //            Vector3 vecB = new Vector3(p.pulseLocation.x, p.pulseLocation.y, 0.0f);

    //            float distanceApart = Mathf.Abs(Vector3.Distance(position, p.pulseLocation));
    //            if (distanceApart > pulseDistanceThreshold)
    //            {
    //                float dist = 1.0f / distanceApart;
    //                dist = Mathf.Min(dist, maxDistanceAffect);
    //                pulsePower += (new Vector3(position.x, position.y, p.pulseLocation.z) - p.pulseLocation).normalized * p.pulseStrength * dist;
    //            }
    //        }
    //    }

    //    float xVal = Mathf.Max(-pulsePower.x,-maxPulseAffect);
    //    float yVal = Mathf.Min(pulsePower.y, maxPulseAffect);
    //    Vector2 returnVec = new Vector2(xVal, yVal);
    //    return returnVec;
    //}

    bool TouchingUI(Vector2 position)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = position;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        return raycastResults.Count == 0;  
    }
    void DidTap()
    {
        bool canTap = !EventSystem.current.IsPointerOverGameObject();
        if (Input.touches.Count() > 0){
            canTap = TouchingUI(Input.touches[0].position);
        }
        if (canTap){
            tapCount++;
            AddScore(tapScoreEffect);
            RemoveActivePulses();
            AddPulse();
            AudioManager.Instance.PlayClip(attractPulseAudio);
            userInterface.SetTapCount(tapCount);
        }
    }
    void CheckPhoneTaps()
    {
        foreach(Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    tapCount++;
                    Vector2 loc = (t.position);
                    Vector3 pulseLocation = new Vector3(loc.x, loc.y, -10.0f);
                    AddPulseAtPosition(pulseLocation);
                    userInterface.SetTapCount(tapCount);
                }
            }
        }
    }
    void CheckForTapComplete()
    {
        if (activePulse != null)
        {
            bool touchIsActive = (Input.GetMouseButton(0) || Input.touches.Count() >0);
            if (!touchIsActive)
            {
                activePulse.dissapates = true;
            }
        }
    }
    private void Update()
    {
        if (!gameComplete)
        {
            seconds += Time.deltaTime;
            userInterface.SetTime(seconds);
            CheckForRemaining();
        }
        if (Input.GetMouseButtonDown(0))
        {
            DidTap();
        }else if (Input.GetMouseButton(0))
        {
            if (activePulse){
                SetPulseToInputPos();
            }
        }
        CheckForTapComplete();
        UpdatePulses();
    }
}
