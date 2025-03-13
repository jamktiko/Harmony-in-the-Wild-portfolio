using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevToolsQuestList : MonoBehaviour
{
    public DevToolQuestStateChanger questStateChangerPrefab;
    public List<QuestScriptableObject> questObjects;


    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (QuestScriptableObject quest in questObjects)
        {
            DevToolQuestStateChanger questStateChanger = Instantiate(questStateChangerPrefab, transform);
            questStateChanger.SetQuest(quest);
            questStateChanger.SetState(QuestManager.Instance.GetQuest(quest).State);
        }
    }
}
