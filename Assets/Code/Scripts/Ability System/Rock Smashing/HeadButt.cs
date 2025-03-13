using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HeadButt : MonoBehaviour
{
    private float _force;
    [SerializeField] private float _finalForce;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _currentRock;

    [SerializeField]
    private bool _isReadyToHeadButt=true;

    [SerializeField] private GameObject _smashingUI;

    private void FixedUpdate()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPerformedThisFrame() && _force != 0 && _currentRock != null&&_isReadyToHeadButt)
        {
            TriggerHeadButt();
        }
        else if (Mouse.current.leftButton.isPressed && _force < 10 && _currentRock != null&&_isReadyToHeadButt)
        {
            _force += 0.05f;
            _slider.value = _force;
        }
        else if (_isReadyToHeadButt)
        {
            _slider.value -= 0.2f;
            _force = _slider.value;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void TriggerHeadButt()
    {
        _finalForce = _force;
        if (_currentRock)
        {
            _isReadyToHeadButt=false;
            _slider.value = _finalForce;
            FoxMovement.Instance.playerAnimator.HeadButt();
            Destructible destructible = _currentRock.GetComponent<Destructible>();
            bool needsSlider = destructible.IsQuestRock;
            float minForce = destructible.MinRequiredForce;
            float maxForce = destructible.MaxRequiredForce;
            if (needsSlider && _finalForce < maxForce && _finalForce > minForce)
            {
                destructible.DestroyQuestRock();
                _currentRock = null;
                _slider.value = 0;
                _force = _slider.value;
                Debug.Log("destroyed");
                if (SmashingQuestStep1.Instance)
                {
                    SmashingQuestStep1.Instance.IncreaseRockSmashCount();
                }
            }
            Debug.Log("Over");

            Invoke("TriggerCooldown",2f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            _currentRock = other.gameObject;
            Destructible currentDestructible = _currentRock.GetComponent<Destructible>();
            if (currentDestructible.IsQuestRock)
            {
                _smashingUI.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            _currentRock = null;
            _smashingUI.SetActive(false);
        }
    }

    private void TriggerCooldown()
    {
        _slider.value = 0f;
        _force = _slider.value;
        _isReadyToHeadButt = true;
    }

}
