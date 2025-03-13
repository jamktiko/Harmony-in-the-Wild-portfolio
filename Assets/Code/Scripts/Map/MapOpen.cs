using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapOpen : MonoBehaviour
{
    public static event Action<bool> OnMapOpen;

    [FormerlySerializedAs("mapPanel")] [SerializeField]
    private GameObject _mapPanel; //Uses now World Map Camera
    [FormerlySerializedAs("mapCam")] [SerializeField]
    private GameObject _mapCam;
    [SerializeField] private GameObject _miniMap;

    [FormerlySerializedAs("mapQuestMarkers")]
    [Header("Indicators")]
    [SerializeField] private Image[] _mapQuestMarkers;

    private UnityEngine.Rendering.Universal.DepthOfField _depthOfField;

    private void Start()
    {
        StartCoroutine(ReEnableMarkers());
    }

    private void Update()
    {
        HandleMapToggle();
        HandleDebugFeatures();
    }

    private void HandleMapToggle()
    {
        if (PlayerInputHandler.Instance.OpenMapInput.WasPressedThisFrame())
        {
            DialogueManager.Instance.CanStartDialogue = !DialogueManager.Instance.CanStartDialogue;

            ToggleMapVisibility();
            UpdateCursorState();
            ToggleDepthOfField();
            ToggleMiniMapVisibility();
        }
    }

    private void ToggleMapVisibility()
    {
        //mapCam.SetActive(!mapCam.activeInHierarchy);
        _mapPanel.SetActive(!_mapPanel.activeInHierarchy);
        OnMapOpen?.Invoke(_mapPanel.activeInHierarchy);
        if (_mapPanel.activeSelf)
            QuestManager.Instance.SetQuestMarkers(_mapQuestMarkers);
    }

    private void ToggleMiniMapVisibility()
    {
        _miniMap.SetActive(!_mapPanel.activeInHierarchy);
    }

    private void UpdateCursorState()
    {
        bool mapIsActive = _mapCam.activeInHierarchy;
        Cursor.lockState = mapIsActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mapIsActive;
    }

    private void ToggleDepthOfField()
    {
        if (_depthOfField != null)
        {
            _depthOfField.active = !_mapCam.activeInHierarchy;
        }
    }

    private void HandleDebugFeatures()
    {
        //if (PlayerInputHandler.instance.JumpInput.WasPressedThisFrame())
        //{
        //    ToggleDebugFeatures();
        //}
    }

    private void ToggleDebugFeatures()
    {
        foreach (Transform child in _mapPanel.transform)
        {
            if (child.name != "Map")
            {
                child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
            }
        }
    }

    private IEnumerator ReEnableMarkers()
    {
        GetComponent<QuestMarkers>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<QuestMarkers>().enabled = true;
    }
}
