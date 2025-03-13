public class DungeonQuestQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceDungeonQuest += CompleteDungeonQuestStep;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceDungeonQuest -= CompleteDungeonQuestStep;
    }

    private void CompleteDungeonQuestStep(QuestScriptableObject questObject)
    {
        if (questObject == QuestObject)
        {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // this quest step doesn't require anything to be saved, as the progress is being tracked based on the amount of completed quest steps
    }
}