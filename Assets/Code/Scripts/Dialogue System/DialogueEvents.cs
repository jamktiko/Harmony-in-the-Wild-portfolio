using System;

public class DialogueEvents
{
    public event Action OnStartDialogue;

    public void StartDialogue()
    {
        OnStartDialogue?.Invoke();
    }

    public event Action OnEndDialogue;

    public void EndDialogue()
    {
        OnEndDialogue?.Invoke();
    }

    public event Action<QuestScriptableObject, int> OnSetMidQuestDialogue;

    public void SetMidQuestDialogue(QuestScriptableObject questObject, int dialogueIndex)
    {
        OnSetMidQuestDialogue?.Invoke(questObject, dialogueIndex);
    }

    public event Action<DialogueVariables> OnChangeDialogueVariable;

    public void ChangeDialogueVariable(DialogueVariables changedVariable)
    {
        OnChangeDialogueVariable?.Invoke(changedVariable);
    }

    public event Action<DialogueQuestNpCs, bool> OnRegisterPlayerNearNpc;

    public void RegisterPlayerNearNpc(DialogueQuestNpCs npc, bool isClose)
    {
        OnRegisterPlayerNearNpc?.Invoke(npc, isClose);
    }
}
