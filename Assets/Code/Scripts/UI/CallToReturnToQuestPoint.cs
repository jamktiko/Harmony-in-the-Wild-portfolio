using UnityEngine;
using UnityEngine.Serialization;

public class CallToReturnToQuestPoint : MonoBehaviour
{
    [FormerlySerializedAs("questInfoForPoint")] [SerializeField] private QuestScriptableObject _questInfoForPoint;
    [FormerlySerializedAs("finalCallToAction")] [SerializeField] private string _finalCallToAction;

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnReturnToSideQuestPoint += UpdateQuestUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnReturnToSideQuestPoint -= UpdateQuestUI;
    }

    private void UpdateQuestUI(QuestScriptableObject questObject)
    {
        // if the corresponding quest is being updated to CAN_FINISH state, use this UI
        if (_questInfoForPoint == questObject)
        {
            GameEventsManager.instance.QuestEvents.ShowQuestUI(_questInfoForPoint, _finalCallToAction, "");
        }
    }
}
