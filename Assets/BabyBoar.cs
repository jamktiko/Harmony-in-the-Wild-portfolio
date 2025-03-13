using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BabyBoar : MonoBehaviour
{
    [SerializeField]private bool _interactable; 
    [SerializeField] public bool PlayerHasBoar;
    SmashingQuestStep3 smashingQuestStep3;
    private void Start()
    {
        smashingQuestStep3 = GameObject.FindObjectOfType<SmashingQuestStep3>();
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPerformedThisFrame()&&_interactable&& QuestManager.Instance.GetQuestById("Smashing").GetCurrentQuestStepIndex()==3)
        {
            PlayerHasBoar = true;
            smashingQuestStep3.CollectBoar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _interactable = false;
        }
    }
}
