using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DevToolQuestStateChanger : MonoBehaviour
{
    internal QuestScriptableObject quest;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TMP_Dropdown _dropdown;

    private QuestState _currentQuestState;

    public void SetQuest(QuestScriptableObject quest)
    {
        this.quest = quest;
        _label.text = this.quest.DisplayName;
        _currentQuestState = QuestManager.Instance.GetQuest(this.quest).State;
        _dropdown.value = (int)_currentQuestState;
    }

    public void ChangeQuestState()
    {
        switch (_dropdown.value)
        {
            case 2:
                GameEventsManager.instance.QuestEvents.StartQuest(quest);
                break;

            case 4:
                GameEventsManager.instance.QuestEvents.FinishQuest(quest);
                break;

            default:
                Debug.Log("No changes coded for this state type.");
                break;
        }
    }

    internal void SetState(QuestState state) => _dropdown.SetValueWithoutNotify((int)state);
}
