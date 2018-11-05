using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public struct MagneticPulse{
        public float pulseStrength;
        public Vector3 pulseLocation;
        public GameObject go;
    }
    public float pulseDistance = 3.0f;
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
            float pStrength = p.pulseStrength - Time.deltaTime * drainSpeed;
            pStrength = Mathf.Max(pStrength, 0.0f);
            p.pulseStrength = pStrength;
            if (p.pulseStrength > 0)
            {
                nextPulseList.Add(p);
                float scaleSize = p.pulseStrength * pulseDistance * 2.0f;
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
        pulse.pulseStrength = 1.0f;

        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pulse.pulseLocation = loc;
        pulse.go = Instantiate(pulseImage, loc, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        pulses.Add(pulse);
    }
    public Vector2 AffectOnPosition(Vector2 position)
    {
        Vector2 pulsePower = new Vector2(0.0f, 0.0f);
        foreach(MagneticPulse p in pulses)
        {
            float dist = Vector3.Distance(position, p.pulseLocation);
            if (dist < pulseDistance)
            {
                float distancePower = (pulseDistance - dist)/pulseDistance;
                pulsePower.x += (p.pulseLocation.x - position.x) / pulseDistance * p.pulseStrength;
                pulsePower.y += (position.y -  p.pulseLocation.y) / pulseDistance * p.pulseStrength;
            }
        }

        return pulsePower;
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
