using UnityEngine;
using UnityEngine.Serialization;

public class ExitCaveTrigger : MonoBehaviour
{
    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;

    private void OnTriggerEnter(Collider other)
    {
        int currentQuestStepIndex = QuestManager.Instance.GetQuest(_questSo).GetCurrentQuestStepIndex();

        if (other.gameObject.CompareTag("Trigger") && currentQuestStepIndex >= 3)
        {
            ExitCaveQuest.Instance.ExitCave();
            GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld);
        }
    }
}
