using System;

public class QuestEvents
{
    public event Action<QuestScriptableObject> OnStartQuest;

    public void StartQuest(QuestScriptableObject questObject)
    {
        OnStartQuest?.Invoke(questObject);
    }

    public event Action<QuestScriptableObject> OnAdvanceQuest;

    public void AdvanceQuest(QuestScriptableObject questObject)
    {
        OnAdvanceQuest?.Invoke(questObject);
        UnityEngine.Debug.Log("Invoked advance quest");

    }

    public event Action<QuestScriptableObject> OnFinishQuest;

    public void FinishQuest(QuestScriptableObject questObject)
    {
        OnFinishQuest?.Invoke(questObject);
        UnityEngine.Debug.Log("Invoked finish quest");

    }

    public event Action<Quest> OnQuestStateChange;

    public void QuestStateChange(Quest quest)
    {
        OnQuestStateChange?.Invoke(quest);
    }

    public event Action<QuestScriptableObject, int, QuestStepState> OnQuestStepStateChange;

    public void QuestStepStateChange(QuestScriptableObject questObject, int stepIndex, QuestStepState questStepState)
    {
        OnQuestStepStateChange?.Invoke(questObject, stepIndex, questStepState);
    }

    public event Action<QuestScriptableObject> OnAdvanceDungeonQuest;

    public void AdvanceDungeonQuest(QuestScriptableObject questObject)
    {
        OnAdvanceDungeonQuest?.Invoke(questObject);
    }

    public event Action<QuestScriptableObject, string, string> OnShowQuestUI;

    public void ShowQuestUI(QuestScriptableObject questObject, string description, string progress)
    {
        OnShowQuestUI?.Invoke(questObject, description, progress);
    }

    public event Action<string> OnUpdateQuestProgressInUI;

    public void UpdateQuestProgressInUI(string progress)
    {
        OnUpdateQuestProgressInUI?.Invoke(progress);
    }

    public event Action OnHideQuestUI;

    public void HideQuestUI()
    {
        OnHideQuestUI?.Invoke();
    }

    public event Action<QuestScriptableObject> OnReturnToSideQuestPoint;

    public void ReturnToSideQuestPoint(QuestScriptableObject questObject)
    {
        OnReturnToSideQuestPoint?.Invoke(questObject);
    }

    public event Action<string> OnChangeActiveQuest;

    public void ChangeActiveQuest(string id)
    {
        OnChangeActiveQuest?.Invoke(id);
    }

    public event Action<DialogueQuestNpCs> OnStartMovingQuestNpc;

    public void StartMovingQuestNpc(DialogueQuestNpCs character)
    {
        OnStartMovingQuestNpc?.Invoke(character);
    }

    public event Action OnReachWhaleDestination;

    public void ReachWhaleDestination()
    {
        OnReachWhaleDestination?.Invoke();
    }

    public event Action<QuestScriptableObject> OnReachTargetDestinationToCompleteQuestStep;

    public void ReachTargetDestinationToCompleteQuestStep(QuestScriptableObject questObject)
    {
        OnReachTargetDestinationToCompleteQuestStep?.Invoke(questObject);
    }

    public event Action<QuestItem> OnCollectQuestItem;

    public void CollectQuestItem(QuestItem item)
    {
        OnCollectQuestItem?.Invoke(item);
    }
}