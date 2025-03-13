public class DestinationQuestStep : QuestStep
{
    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuest(), Objective, Progress);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnReachTargetDestinationToCompleteQuestStep += MarkQuestAsCompleted;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnReachTargetDestinationToCompleteQuestStep -= MarkQuestAsCompleted;
    }

    private void MarkQuestAsCompleted(QuestScriptableObject id)
    {
        if (id == QuestObject)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        // nothing to update
    }

    protected override void SetQuestStepState(string state)
    {
        // nothing to update
    }
}
