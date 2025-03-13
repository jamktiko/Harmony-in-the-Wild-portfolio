using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestMarkers : MonoBehaviour
{
    //list of buttons & quest indicators?
    //index of button matches index of quest
    //when quest finished, disable the corresponding indicator and enable button based on given index

    [FormerlySerializedAs("mapButtons")] [SerializeField] private List<GameObject> _mapButtons = new List<GameObject>();
    [FormerlySerializedAs("mapIndicators")] [SerializeField] private List<GameObject> _mapIndicators = new List<GameObject>();

    private Dictionary<QuestScriptableObject, int> _idToIndex = new Dictionary<QuestScriptableObject, int>();
    private Quest _quest;
    private int _questIndex;

    private void Start()
    {
        Invoke("GrabQuestIds", 1f);
    }

    private void GrabQuestIds()
    {
        int index = 0;
        foreach (QuestScriptableObject questObject in QuestManager.QuestMap.Keys)
        {
            _idToIndex.Add(questObject, index);

            index++;
        }
    }
    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnFinishQuest += UnlockMapTeleport;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= UnlockMapTeleport;
    }

    private void UnlockMapTeleport(QuestScriptableObject id)
    {
        //Debug.Log("string id is: " + id);

        _quest = QuestManager.Instance.GetQuest(id);
        _questIndex = GetIndex(_quest.Info);

        //Debug.Log(mapButtons[questIndex]);
        //Debug.Log(mapIndicators[questIndex]);
        //mapButtons[questIndex].SetActive(true);
        //mapIndicators[questIndex].SetActive(false);

        //Debug.Log("QuestMarker says: questIndex - " + questIndex);

        foreach (var pair in _idToIndex)
        {
            //Debug.Log("Key: " + pair.Key + ", Value: " + pair.Value);
        }

    }
    private void Update()
    {
        //int questIndex = 0;

        //foreach (Quest quest in QuestManager.instance.questMap.Values)
        //{
        //    if (quest.state == QuestState.FINISHED)
        //    {
        //        questIndex = GetIndexFromId(quest.info.id);
        //        //Debug.Log($"QuestMarker says: quest.info.id is - {quest.info.id}. questIndex is - {questIndex}.");
        //        //mapButtons[questIndex].SetActive(true);
        //        //mapIndicators[questIndex].SetActive(false);
        //    }
        //}
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
