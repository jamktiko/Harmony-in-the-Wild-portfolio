using UnityEngine;
using UnityEngine.VFX;

public class WaterExitEventController : MonoBehaviour
{
    [SerializeField] public VisualEffect vfxGraph;
    internal readonly static int beginExitWaterEventHash = Shader.PropertyToID("OnExitWater");
    internal readonly static int shakeWaterLeftEventHash = Shader.PropertyToID("OnShakeWaterLeft");
    internal readonly static int shakeWaterRightEventHash = Shader.PropertyToID("OnShakeWaterRight");
    internal readonly static int endExitWaterEventHash = Shader.PropertyToID("OnWaterExited");

    public void Awake()
    {
        if(vfxGraph == null)
        {
            Debug.LogWarning("VFX Graph reference is missing!", gameObject);
            enabled = false;
            return;
        }
        vfxGraph.SendEvent(endExitWaterEventHash);
    }

    public void BeginExitWater()
    {
        if (vfxGraph != null)
        {
            vfxGraph?.SendEvent(beginExitWaterEventHash);
        }
    }

    public void ShakeWaterLeft()
    {
        if (vfxGraph != null)
        {
            vfxGraph.SendEvent(shakeWaterLeftEventHash);
        }
        else
        {
            Debug.LogWarning("VFX Graph reference is missing!");
        }
    }

    public void ShakeWaterRight()
    {
        if (vfxGraph != null)
        {
            vfxGraph.SendEvent(shakeWaterRightEventHash);
        }
        else
        {
            Debug.LogWarning("VFX Graph reference is missing!");
        }
    }

    public void EndExitWater()
    {
        if (vfxGraph != null)
        {
            vfxGraph.SendEvent(endExitWaterEventHash);
        }
        else
        {
            Debug.LogWarning("VFX Graph reference is missing!");
        }
    }
}
