﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public struct MagneticPulse{
        public float pulseStrength;
        public float pulseDuration;
        public Vector3 pulseLocation;
        public GameObject go;
    }
    public float pulseDistance = 3.0f;
    public float pulseDisplaySize = 4.0f;
    public float pulseStrength = 4.0f;
    public float magneticField = 0.0f;
    public float affectLimit = 2.5f;
    public float drainSpeed = 1.0f;
    public float pressedAmount = 0.0f;
    public List<MagneticPulse> pulses = new List<MagneticPulse>();
    public GameObject pulseImage;

    public Transform leftLimit;
    public Transform rightLimit;
    void UpdatePulses()
    {
        List<MagneticPulse> nextPulseList = new List<MagneticPulse>();
        for (int i = 0; i < pulses.Count; i++)
        {
            MagneticPulse p = pulses[i];
            float pDuration = p.pulseDuration - Time.deltaTime * drainSpeed;
            pDuration = Mathf.Max(pDuration, 0.0f);
            p.pulseDuration = pDuration;
            if (p.pulseDuration > 0)
            {
                nextPulseList.Add(p);
                float scaleSize = pulseDisplaySize * p.pulseDuration;
                p.go.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
            }
            else
            {
                Destroy(p.go);
            }
        }
        pulses = nextPulseList;

    }
    void AddPulse()
    {
        MagneticPulse pulse = new MagneticPulse();
        pulse.pulseStrength = pulseStrength;
        pulse.pulseDuration = 1.0f;
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pulseLocation = new Vector3(loc.x, loc.y, -10.0f);
        pulse.pulseLocation = loc;
        pulse.go = Instantiate(pulseImage, pulseLocation, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        pulses.Add(pulse);
    }
    public Vector2 AffectOnPosition(Vector2 position)
    {
        Vector3 pulsePower = new Vector2(0.0f, 0.0f);
        foreach(MagneticPulse p in pulses)
        {
            float divideBy = Mathf.Abs(Vector3.Distance(position, p.pulseLocation));
            float dist = divideBy < 0.5f ? 0.0f : 1.0f / divideBy;
            pulsePower += (new Vector3(position.x, position.y, p.pulseLocation.z) - p.pulseLocation).normalized * pulseStrength * dist;
            //float dist = Vector3.Distance(position, p.pulseLocation);
            //if (dist < pulseDistance)
            //{
            //    float distancePower = Mathf.Abs((pulseDistance - dist)/pulseDistance);
            //    //float xVal = 1.0f / (p.pulseLocation.x - position.x) * distancePower;
            //    //float yVal = 1.0f / (position.y - p.pulseLocation.y) * distancePower;

            //    pulsePower.x += (p.pulseLocation.x - position.x) / pulseDistance * p.pulseStrength;
            //    pulsePower.y += (position.y - p.pulseLocation.y) / pulseDistance * p.pulseStrength;

            //    //pulsePower.x += xVal;
            //    //pulsePower.y += yVal;
            //}
        }

        return new Vector2(-pulsePower.x, pulsePower.y);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddPulse();
        }
        UpdatePulses();
        //float pulseAdd = 0.0f;
        //if (Input.GetMouseButtonDown(1))
        //{
        //    pulseAdd += 1.0f;
        //    pressedAmount = 100.0f;

        //}else if (Input.GetMouseButtonDown(0))
        //{
        //    pulseAdd -= 1.0f;
        //    pressedAmount = 100.0f;
        //}

        //float newField = magneticField + pulseAdd;
        //newField = Mathf.Max(newField, -affectLimit);
        //newField = Mathf.Min(newField, affectLimit);
        //magneticField = newField;

        //float newPressedAmt = pressedAmount -= Time.deltaTime * 70.0f;
        //pressedAmount = Mathf.Max(0, newPressedAmt);
    
        //float diminishAmount = drainSpeed * Time.deltaTime;
        //if (magneticField < 0) diminishAmount *= -1.0f;
        //magneticField -= diminishAmount;

    }
}
