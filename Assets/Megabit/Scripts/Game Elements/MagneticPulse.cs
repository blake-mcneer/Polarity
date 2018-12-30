using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagneticPulse : ScriptableObject {
    public float pulseStrength;
    public float pulseDuration;
    public float pulseRemaining;
    public bool dissapates = true;
    public Vector3 pulseLocation;
    public GameObject prefab;
    public int index;
    public List<int> specificIndeces = new List<int>();
    public bool hasBeenReached;
    public bool isDisabled = false;
        
    public void Dissapate()
    {
        ParticleSystem pSystem = prefab.GetComponent<ParticleSystem>();
        var sys = pSystem.main;
        isDisabled = true;
        //sys.maxParticles = 10;
        sys.simulationSpeed = 10.0f;
        sys.startColor = Color.black;
        var emission = pSystem.emission;
        emission.rateOverTime = 1.0f;
        pulseStrength = 0.0f;
    }


}
