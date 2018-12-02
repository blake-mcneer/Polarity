using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {


    public float pulseDistance = 3.0f;
    public float pulseDurationSetting = 5.0f;
    public float pulseDisplaySize = 4.0f;
    public float pulseStrength = 4.0f;
    public float pulseDistanceThreshold = .05f;
    public float affectLimit = 2.5f;
    public float drainSpeed = 1.0f;
    public bool gameComplete = false;
    public int score = 0;
    int tapCount = 0;
    float seconds = 0.0f;
    float maxDistanceAffect = 0.75f;
    UI userInterface;
    [HideInInspector] public List<MagneticPulse> preconfiguredPulses = new List<MagneticPulse>();
    [HideInInspector] public List<MagneticPulse> attractionPulses = new List<MagneticPulse>();
    [HideInInspector] public List<MagneticPulse> repulsionPulses = new List<MagneticPulse>();
    public GameObject pulseImage;
    public GameObject repulsionImage;
    public Transform leftLimit;
    public Transform rightLimit;
    int pulseIndex = 1;
    List<int> indexList = new List<int>();
    MagneticObject[] magneticObjects;
    //MagneticPulse activePulse;

    public void SetupManager()
    {
        for (int i = 0; i < attractionPulses.Count; i++){
            attractionPulses[i].pulseStrength = 0.0f;
            Destroy(attractionPulses[i].prefab);
        }
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
        foreach (MagneticObject obj in magneticObjects)
        {
            returnList.Add(obj.index);
        }
        return returnList;
    }

    private void Start()
    {
        userInterface = FindObjectOfType<UI>();
        SetupManager();
    }
    void LoadMagneticObjects()
    {        
        magneticObjects = FindObjectsOfType<MagneticObject>();
        int index = 0;
        foreach (MagneticObject mag in magneticObjects)
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
            float pDuration = p.pulseRemaining - Time.deltaTime * drainSpeed;
            pDuration = Mathf.Max(pDuration, 0.0f);
            p.pulseRemaining = pDuration;
            if (p.pulseRemaining > 0)
            {
                nextPulseList.Add(p);
                float scaleSize = pulseDisplaySize * p.pulseRemaining/p.pulseDuration;
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
    public void AddScore(int scoreAdjustment)
    {
        score += scoreAdjustment;
        userInterface.SetScore(score);
    }
    public void AddRepulsion(Vector3 pos, int[] affectedIndeces)
    {
        MagneticPulse p = CreatePulse(position: pos, strength: -pulseStrength, duration: 0.75f, prefab: repulsionImage);
        p.specificIndeces = affectedIndeces.ToList();
        attractionPulses.Add(p);
    }
    public void RemoveFromCurrentAttractionPulses(int index)
    {
        foreach (MagneticPulse p in attractionPulses)
        {
            if (p.pulseStrength > 0.0f && p.specificIndeces != null){
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
        pulse.prefab = Instantiate(prefab, pulse.pulseLocation, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        return pulse;
    }
    void AddPulseAtPosition(Vector3 pos)
    {
        if (Time.timeScale > 0){
            MagneticPulse p = CreatePulse(position: pos, strength: pulseStrength, duration: pulseDurationSetting, prefab: pulseImage);
            p.specificIndeces = FullIndexList();
            attractionPulses.Add(p);
        }

    }
    void RemoveActivePulses()
    {
        foreach (MagneticPulse p in attractionPulses){
            p.Dissapate();
        }
    }

    void AddPulse()
    {
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pulseLocation = new Vector3(loc.x, loc.y, 10.0f);
        AddPulseAtPosition(pulseLocation);
    }
    public Vector2 AffectOnPosition(Vector2 position, int objectIndex)
    {
        if (pulseDistanceThreshold <= 0.0f) pulseDistanceThreshold = 0.1f;

        Vector3 pulsePower = new Vector2(0.0f, 0.0f);
        List<MagneticPulse> pulsesReached = new List<MagneticPulse>();
        for (int i = 0; i < attractionPulses.Count; i++)
        {
            MagneticPulse p = attractionPulses[i];
            if (p.specificIndeces == null || p.specificIndeces.Contains(objectIndex))
            {
                Vector3 vecA = new Vector3(position.x, position.y, 0.0f);
                Vector3 vecB = new Vector3(p.pulseLocation.x,p.pulseLocation.y, 0.0f);

                float distanceApart = Mathf.Abs(Vector3.Distance(vecA, vecB));
                if (distanceApart > pulseDistanceThreshold)
                {
                    float dist = 1.0f / distanceApart;
                    dist = Mathf.Min(dist, maxDistanceAffect);
                    float pulseAffect = p.pulseRemaining / p.pulseDuration;
                    pulsePower += (new Vector3(position.x, position.y, p.pulseLocation.z) - p.pulseLocation).normalized * p.pulseStrength *pulseAffect*dist;
                }
                else{
                    //This would shrink the pulse away if reached
                    //p.Dissapate();

                }
            }
        }

        foreach (MagneticPulse p in preconfiguredPulses)
        {
            Vector3 vecA = new Vector3(position.x, position.y, 0.0f);
            Vector3 vecB = new Vector3(p.pulseLocation.x, p.pulseLocation.y, 0.0f);

            float distanceApart = Mathf.Abs(Vector3.Distance(position, p.pulseLocation));
            if (distanceApart > pulseDistanceThreshold)
            {
                float dist =  1.0f / distanceApart;
                dist = Mathf.Min(dist, maxDistanceAffect);
                pulsePower += (new Vector3(position.x, position.y, p.pulseLocation.z) - p.pulseLocation).normalized * p.pulseStrength * dist;
            }
        }

        return new Vector2(-pulsePower.x, pulsePower.y);
    }
    void DidTap()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            tapCount++;
            RemoveActivePulses();
            AddPulse();
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
    private void Update()
    {
        if (!gameComplete)
        {
            seconds += Time.deltaTime;
            userInterface.SetTime(seconds);
        }
        if (Input.GetMouseButtonDown(0))
        {
            DidTap();
        }
//        CheckPhoneTaps();

        UpdatePulses();
    }
}
