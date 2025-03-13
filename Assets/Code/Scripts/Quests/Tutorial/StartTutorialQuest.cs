using UnityEngine;
using UnityEngine.Serialization;

public class StartTutorialQuest : MonoBehaviour
{ 
    [SerializeField] private QuestScriptableObject _questSo;

    private void Start()
    {
        if (QuestManager.Instance.CheckQuestState(_questSo) == QuestState.CanStart)
        {
            GameEventsManager.instance.QuestEvents.StartQuest(_questSo);
            Destroy(this);
        }

        else
        {
            Debug.Log("Not starting tutorial quest, current state: " + QuestManager.Instance.CheckQuestState(_questSo));
            Destroy(this);
        }
    }
}
