using System;
using UnityEngine;

public class IntroCameraEvents : MonoBehaviour
{
    public static event Action IntroCameraStarted;
    public static event Action IntroCameraEnded;

    private bool _cameraAnimationStarted = false;
    private bool _cameraAnimationStopped = false;

    public void IntroCameraStart()
    {
        if (!_cameraAnimationStarted)
        {
            IntroCameraStarted?.Invoke();
            _cameraAnimationStarted = true;
        }
    }

    public void IntroCameraStop() 
    {
        if (!_cameraAnimationStopped)
        {
            IntroCameraEnded?.Invoke();
            _cameraAnimationStopped = true;
        }
    }

}
