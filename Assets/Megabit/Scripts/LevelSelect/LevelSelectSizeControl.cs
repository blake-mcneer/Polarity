using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSizeControl : MonoBehaviour {
    public GameObject TopBanner;
    public GameObject LevelSection;
    void Start () {
        ResizeComponents();
	}
    void ResizeComponents()
    {

        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(Vector3.one);
        RectTransform tForm = TopBanner.GetComponent<RectTransform>();
        float bannerSize = Screen.height * GameSingleton.Instance.bannerPercentage;
        tForm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bannerSize);

        float levelSectionSize = Screen.height * (1.0f - GameSingleton.Instance.bannerPercentage);
        RectTransform levelsRect = LevelSection.GetComponent<RectTransform>();
        levelsRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, levelSectionSize);
    }
}
