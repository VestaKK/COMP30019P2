using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessing : MonoBehaviour
{
    [SerializeField] public Material postProcessingMat;

    [SerializeField] float _chromaticAmountMax = 0.02f;

    private float _chromaticAbbIntensity;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postProcessingMat);
    }

    public void SetChromaticAbberationIntensity(float healthPercentage) 
    {
        _chromaticAbbIntensity = (1 - healthPercentage) * _chromaticAmountMax;
    }

    private void Update()
    {
        postProcessingMat.SetFloat("_Amount", _chromaticAbbIntensity);
    }
}
