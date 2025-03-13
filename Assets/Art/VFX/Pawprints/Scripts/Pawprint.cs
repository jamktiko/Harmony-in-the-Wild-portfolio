using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pawprint : MonoBehaviour
{
    public float fadeTime = 10f;
    internal float spawnTime;
    private DecalProjector _urpDecalProjector;
    private Material _material;
    private int _alphaHash = Shader.PropertyToID("_Alpha");

    private void Start()
    {
        _urpDecalProjector = GetComponent<DecalProjector>();

        // Making copy of the material so every pawprint can have its own alpha value
        _material = new Material(_urpDecalProjector.material);
        _urpDecalProjector.material = _material;
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
        if(_material != null)
            _material.SetFloat(_alphaHash, 0);
    }

    private void Update()
    {
        if (fadeTime <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (Time.time - spawnTime <= fadeTime)
        {
            _material.SetFloat(_alphaHash, Mathf.Clamp01((Time.time - spawnTime) / fadeTime));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void OnValidate()
    {
        if(fadeTime <= 0)
        {
            fadeTime = 1;
        }
    }
}