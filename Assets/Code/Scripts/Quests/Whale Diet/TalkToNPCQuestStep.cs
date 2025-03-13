using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TalkToNpcQuestStep : QuestStep
{
    [FormerlySerializedAs("dialogueToPassForProgress")] [SerializeField]
    private DialogueVariables _dialogueToPassForProgress;
    [FormerlySerializedAs("midQuestDialogueIndex")] [SerializeField] private int _midQuestDialogueIndex = 0;
    [FormerlySerializedAs("character")] [SerializeField] private DialogueQuestNpCs _character = DialogueQuestNpCs.Whale;
    protected bool _canProgressQuest;

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuest(), Objective, Progress);
        GameEventsManager.instance.DialogueEvents.SetMidQuestDialogue(GetQuest(), _midQuestDialogueIndex);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.DialogueEvents.OnChangeDialogueVariable += CheckProgressInDialogue;
        GameEventsManager.instance.DialogueEvents.OnRegisterPlayerNearNpc += PlayerIsClose;
        SceneManager.sceneLoaded += SetInfoInOverworld;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.DialogueEvents.OnChangeDialogueVariable -= CheckProgressInDialogue;
        GameEventsManager.instance.DialogueEvents.OnRegisterPlayerNearNpc -= PlayerIsClose;
        SceneManager.sceneLoaded -= SetInfoInOverworld;
    }

    private void SetInfoInOverworld(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.DialogueEvents.SetMidQuestDialogue(GetQuest(), _midQuestDialogueIndex);
            GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuest(), Objective, Progress);
        }
    }

    private void CheckProgressInDialogue(DialogueVariables changedVariable)
    {
        if (changedVariable == _dialogueToPassForProgress && _canProgressQuest)
        {
            FinishQuestStep();
        }

        else
        {
            Debug.Log("Not able to finish the quest step, values not matching.");
        }
    }

    private void PlayerIsClose(DialogueQuestNpCs npc, bool isClose)
    {
        if (npc == _character)
        {
            _canProgressQuest = isClose;
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