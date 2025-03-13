using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingQuestStep3 : QuestStep
{
    [SerializeField]private GameObject _boarBaby;
    public BabyBoar BabyBoar;

    private void Start()
    {
        var activeBaby = Instantiate(_boarBaby);
        BabyBoar = activeBaby.GetComponent<BabyBoar>();
    }

    public void CollectBoar()
    {
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = BabyBoar.PlayerHasBoar.ToString();
        ChangeState(state);
    }
    protected override void SetQuestStepState(string state)
    {
        BabyBoar.PlayerHasBoar=System.Convert.ToBoolean(state);
        
        UpdateState();
    }
}
