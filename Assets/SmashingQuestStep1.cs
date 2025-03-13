using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingQuestStep1 : QuestStep
{
    public int RockSmashCount;
    public static SmashingQuestStep1 Instance;

    void Awake()
    {
        Instance = this;
    }

    
    public void IncreaseRockSmashCount()
    {
        RockSmashCount++;
        UpdateState();
        if (RockSmashCount==3)
        {
            FinishQuestStep();
        }
    }
    private void UpdateState()
    {
        string state = RockSmashCount.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        RockSmashCount = System.Int32.Parse(state);
        UpdateState();
    }
}
