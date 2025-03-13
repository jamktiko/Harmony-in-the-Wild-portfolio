using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class QuestStep : MonoBehaviour
{
    private bool _isFinished = false;

    private QuestScriptableObject _questObject;
    public QuestScriptableObject QuestObject { get => _questObject; }
    private int _stepIndex;

    [FormerlySerializedAs("positionInScene")]
    [Header("Fill only if quest requires a waypoint")]

    [SerializeField] public Vector3 PositionInScene;

    [FormerlySerializedAs("hasWaypoint")] [SerializeField] public bool HasWaypoint;

    //public string stepName;

    [FormerlySerializedAs("objective")] [Header("Fill for Side Quests Only")]

    public string Objective;

    [FormerlySerializedAs("progress")] [Tooltip("Set in the form as 'Apples collected '; rest is autofilled through script")]

    public string Progress;


    public void InitializeQuestStep(QuestScriptableObject questObject, int stepIndex, string questStepState)
    {
        _questObject = questObject;
        this._stepIndex = stepIndex;

        var q = QuestManager.GetQuestObject("Whale Diet");
        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
        else if (questObject == q)
        {
            SetQuestStepState("0");
        }
        SaveManager.Instance.SaveGame();
    }

    [Obsolete("Use QuestObject.Id instead")]
    protected QuestScriptableObject GetQuest()
    {
        return _questObject;
    }

    protected void FinishQuestStep()
    {
        if (!_isFinished)
        {
            _isFinished = true;

            GameEventsManager.instance.QuestEvents.AdvanceQuest(_questObject);
            Debug.Log("Finished quest step: " + _questObject);
            Invoke("DestroyObject", 0);
        }
    }

    protected void ChangeState(string newState)
    {
        GameEventsManager.instance.QuestEvents.QuestStepStateChange(_questObject, _stepIndex, new QuestStepState(newState));
        SaveManager.Instance.SaveGame();
    }

    protected void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected abstract void SetQuestStepState(string state);
}
