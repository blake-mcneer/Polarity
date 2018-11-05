using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarizedWall : MonoBehaviour {

    private float _size = 0.0f;
    GameManager manager;
    SkinnedMeshRenderer renderer;
    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        renderer = GetComponent<SkinnedMeshRenderer>();
    }
    private void Update()
    {
        renderer.SetBlendShapeWeight(0, manager.pressedAmount);
    }
}
