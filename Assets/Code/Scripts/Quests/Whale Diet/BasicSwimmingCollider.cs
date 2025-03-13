using UnityEngine;
using UnityEngine.Serialization;

public class BasicSwimmingCollider : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("quest")] private QuestScriptableObject _quest;

    private QuestState _currentState;

    private void Start()
    {
        CheckQuestProgressStatus();

        if (_currentState != QuestState.Finished)
        {
            return;
        }

        else
        {
            DisableQuestColliders();
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnFinishQuest += UpdateQuestProgressStatus;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= UpdateQuestProgressStatus;
    }

    private void CheckQuestProgressStatus()
    {
        _currentState = QuestManager.Instance.CheckQuestState(_quest);
    }

    private void DisableQuestColliders()
    {
        gameObject.SetActive(false);
    }

    private void UpdateQuestProgressStatus(QuestScriptableObject questObject)
    {
        if (questObject == _quest)
        {
            CheckQuestProgressStatus();

            if (_currentState == QuestState.Finished)
            {
                DisableQuestColliders();
            }
        }
    }
}
