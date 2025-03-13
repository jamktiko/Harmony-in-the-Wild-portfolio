using UnityEngine;
using UnityEngine.Serialization;

public class AppleCheck : MonoBehaviour
{
    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;

    private void Start()
    {
        QuestState questState = QuestManager.Instance.CheckQuestState(_questSo);

        if (questState == QuestState.Finished || QuestManager.Instance.GetQuest(_questSo).GetCurrentQuestStepIndex() > 0)
        {
            gameObject.SetActive(false);
        }
    }
}
