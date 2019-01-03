using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    public Material TransitionMaterial;
    public float cutoffVal = 0.0f;
    public float fadeVal = 0.0f;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }
    private void Update()
    {
        TransitionMaterial.SetFloat("_Cutoff", cutoffVal);
        TransitionMaterial.SetFloat("_Fade", fadeVal);
    }
}
