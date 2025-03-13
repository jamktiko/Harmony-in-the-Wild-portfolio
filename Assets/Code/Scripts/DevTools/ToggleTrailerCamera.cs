using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ToggleTrailerCamera : MonoBehaviour
{
#if DEBUG
    [SerializeField] private GameObject _trailerCamera; 
    [SerializeField] private List<GameObject> _playerCameras; 
    [SerializeField] private FoxMovement _foxMove;

    private bool _trailerCameraOn;

    private void Update()
    {
        if (PlayerInputHandler.Instance.DebugTrailerCameraToggle.WasPressedThisFrame())
        {
            ToggleCameras();
        }
    }

    private void ToggleCameras()
    {
        _trailerCameraOn = !_trailerCameraOn;

        if (_trailerCameraOn)
        {
            foreach (GameObject camera in _playerCameras)
            {
                camera.SetActive(false);
            }

            _foxMove.enabled = false;
            _trailerCamera.SetActive(true);
        }

        else
        {
            foreach (GameObject camera in _playerCameras)
            {
                camera.SetActive(true);
            }

            _foxMove.enabled = true;
            _trailerCamera.SetActive(false);
        }
    }
#endif
}
