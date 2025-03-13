using System.Collections;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private float _fogDensityForCinematic;
    [SerializeField] private float _fogDecreaseTransitionDuration = 2f;
    [SerializeField] private float _fogInreaceTransitionDuration = 2f;

    //Values from rendering settings
    private float _defaultFogDensity;
    
    private void Awake()
    {
        GetFogRenderingSettings();
    }

    private void OnEnable()
    {
        IntroCameraEvents.IntroCameraStarted += SetFogForCinematic;
        IntroCameraEvents.IntroCameraEnded += ResetFogValues;
    }

    private void GetFogRenderingSettings()
    {
        _defaultFogDensity = RenderSettings.fogDensity;
        Debug.Log("Fog density: " + RenderSettings.fogDensity);
    }

    private void SetFogForCinematic()
    {
        RenderSettings.fogDensity = _fogDensityForCinematic;
        StartCoroutine(FogDecreaseLoop());
    }

    private void ResetFogValues()
    {
        Debug.Log("Setting default values for the fog.");
        RenderSettings.fogDensity = _defaultFogDensity;
        StartCoroutine(FogResetLoop());
    }

    private IEnumerator FogDecreaseLoop()
    {
        float elapsedTime = 0f;
        float startDensity = _fogDensityForCinematic;


        while (elapsedTime < _fogDecreaseTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fogDecreaseTransitionDuration;

            RenderSettings.fogDensity = Mathf.Lerp(_defaultFogDensity, _fogDensityForCinematic, t);
            Debug.Log("Fog density: " + _fogDensityForCinematic);
            yield return null;
        }
    }


    private IEnumerator FogResetLoop()
    {
        float elapsedTime = 0f;
        float startDensity = _fogDensityForCinematic;


        while (elapsedTime < _fogInreaceTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fogInreaceTransitionDuration;

            RenderSettings.fogDensity = Mathf.Lerp(startDensity, _defaultFogDensity, t);
            Debug.Log("Fog density: " + _fogDensityForCinematic);
            yield return null;
        }
    }


    private void OnDisable()
    {
        IntroCameraEvents.IntroCameraStarted -= SetFogForCinematic;
        IntroCameraEvents.IntroCameraEnded -= ResetFogValues;
    }
}
