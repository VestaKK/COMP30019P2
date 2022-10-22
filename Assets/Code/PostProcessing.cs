using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessing : MonoBehaviour
{   
    [SerializeField] public Material postProcessingMat;

    [SerializeField] float _chromaticAmountMax = 0.02f;
    [SerializeField] float _rate = 1;
    [SerializeField] float _amplitude = 0.3f;

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
        postProcessingMat.SetFloat("_Amount", Mathf.Clamp(_chromaticAbbIntensity + _amplitude *
                                            _chromaticAbbIntensity * 
                                            Mathf.Sin(Time.time * _rate), 0.0f, _chromaticAmountMax));
    }
}
