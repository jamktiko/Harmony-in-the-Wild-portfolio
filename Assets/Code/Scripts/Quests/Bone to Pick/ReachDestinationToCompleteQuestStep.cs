using UnityEngine;
using UnityEngine.Serialization;

public class ReachDestinationToCompleteQuestStep : MonoBehaviour
{
    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;
    [FormerlySerializedAs("targetQuestStepIndex")] [SerializeField] private int _targetQuestStepIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            CheckQuestProgress();
        }
    }

    private void CheckQuestProgress()
    {
        int currentQuestStepIndex = QuestManager.Instance.GetQuest(_questSo).GetCurrentQuestStepIndex();

        if (currentQuestStepIndex == _targetQuestStepIndex)
        {
            GameEventsManager.instance.QuestEvents.ReachTargetDestinationToCompleteQuestStep(_questSo);
        }

        else
        {
            Debug.Log($"Trying to progress {_questSo} by entering a trigger collider; current quest step index not matching.");
        }
    }
}
