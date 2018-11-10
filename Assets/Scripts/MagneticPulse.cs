using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagneticPulse : MonoBehaviour {
        public float pulseStrength;
        public float pulseDuration;
        public float pulseRemaining;
        public Vector3 pulseLocation;
        public GameObject prefab;
        public int index;
        public int[] specificIndeces;
        public bool hasBeenReached;
        
}
