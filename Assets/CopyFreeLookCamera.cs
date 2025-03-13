using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CopyFreeLookCamera : MonoBehaviour
{
    [SerializeField]private CinemachineFreeLook _freeLookCamera;
    private void OnEnable()
    {
        CinemachineFreeLook teleGrabCamera = GetComponent<CinemachineFreeLook>();
        teleGrabCamera.m_XAxis.m_MaxSpeed=_freeLookCamera.m_XAxis.m_MaxSpeed;
        teleGrabCamera.m_YAxis.m_MaxSpeed = _freeLookCamera.m_YAxis.m_MaxSpeed;
        teleGrabCamera.m_XAxis.m_InvertInput = _freeLookCamera.m_XAxis.m_InvertInput;
        teleGrabCamera.m_YAxis.m_InvertInput = _freeLookCamera.m_YAxis.m_InvertInput;
    }
}
