using UnityEngine;
using UnityEngine.Serialization;

public class EnableBearDialogueAfterTutorial : MonoBehaviour
{
    [FormerlySerializedAs("tutorialQuest")] [SerializeField] private QuestScriptableObject _tutorialQuest;
    [FormerlySerializedAs("npcDialogue")] [SerializeField] private NpcDialogue _npcDialogue;

    private void Start()
    {
        EnableDialogue(_tutorialQuest);

        GameEventsManager.instance.QuestEvents.OnFinishQuest += EnableDialogue;
    }

    private void OnDisable()
    {

        GameEventsManager.instance.QuestEvents.OnFinishQuest -= EnableDialogue;
    }

    private void EnableDialogue(QuestScriptableObject questObject)
    {
        QuestState state = QuestManager.Instance.CheckQuestState(questObject);

        if (state == QuestState.Finished)
        {
            _npcDialogue.enabled = true;
        }
    }
}