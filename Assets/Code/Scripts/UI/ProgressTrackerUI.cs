using System.Collections.Generic;
using UnityEngine;

public class ProgressTrackerUI : MonoBehaviour, IEventListener<bool>
{
    [SerializeField, Tooltip("{active} and {total} will be replaces with counted value")]
    private string _progressText = "Levers activated {active}/{total}";
    [SerializeField]List<BoolEventChannel> _events;

    private OverrideQuestUI _questUI;
    HashSet<object> _progressObjects = new HashSet<object>();

    private void Awake()
    {
        _questUI = GetComponent<OverrideQuestUI>();
    }

    private void OnEnable()
    {
        foreach(var e in _events)
        {
            e.Register(this);
        }
    }

    private void OnDisable()
    {
        foreach (var e in _events)
        {
            e.Unregister(this);
        }
    }

    public void OnEventRaised(object sender, bool value)
    {
        if (value)
            _progressObjects.Add(sender);
        else
            _progressObjects.Remove(sender);

        UpdateUI();
    }

    private void UpdateUI()
    {
        var progressText = _progressText.Replace("{active}", _progressObjects.Count.ToString());
        progressText = progressText.Replace("{total}", _events.Count.ToString());
        _questUI.Progress = progressText;
    }
}
