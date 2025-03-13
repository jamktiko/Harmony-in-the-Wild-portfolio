using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestSearchArea : MonoBehaviour
{
    [FormerlySerializedAs("whaleSearchArea")] [SerializeField] private GameObject _whaleSearchArea;
    [FormerlySerializedAs("ghostSearchArea")] [SerializeField] private GameObject _ghostSearchArea;
    [FormerlySerializedAs("boneSearchArea")] [SerializeField] private GameObject _boneSearchArea;

    private Dictionary<QuestScriptableObject, int> _idToIndex = new Dictionary<QuestScriptableObject, int>();
    private Quest _quest;

    private void Start()
    {
        Invoke("GrabQuestIds", 1f);
    }

    private void GrabQuestIds()
    {
        int index = 0;
        foreach (QuestScriptableObject questId in QuestManager.QuestMap.Keys)
        {
            _idToIndex.Add(questId, index);

            index++;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest += ToggleSearchAreaOnMinimap;
        GameEventsManager.instance.QuestEvents.OnFinishQuest += ToggleSearchAreaOnMinimap;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest -= ToggleSearchAreaOnMinimap;
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= ToggleSearchAreaOnMinimap;
    }

    public void ToggleSearchAreaOnMinimap(QuestScriptableObject id)
    {
        _quest = QuestManager.Instance.GetQuest(id);
        int questIndex = GetIndex(_quest.Info);

        ToggleSearchArea(questIndex);
    }

    private void ToggleSearchArea(int questIndex)
    {
        //Whale == 16
        //Ghost == 4
        //Bone == 1

        if (questIndex == 16)
        {
            _whaleSearchArea.SetActive(!_whaleSearchArea.activeInHierarchy);
        }
        else if (questIndex == 4)
        {
            _ghostSearchArea.SetActive(!_ghostSearchArea.activeInHierarchy);
        }
        else if (questIndex == 1)
        {
            _boneSearchArea.SetActive(!_boneSearchArea.activeInHierarchy);
        }
    }

    private int GetIndex(QuestScriptableObject id)
    {
        if (_idToIndex.ContainsKey(id))
        {
            return _idToIndex[id];
        }
        else
        {
            throw new KeyNotFoundException("ID not found: " + id);
        }
    }
}
