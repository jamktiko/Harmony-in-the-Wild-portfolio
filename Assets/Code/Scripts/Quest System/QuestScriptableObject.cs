using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
public class QuestScriptableObject : ScriptableObject
{
    [SerializeField, FormerlySerializedAs("id")] private string _id;
    [Obsolete("Use QuestScriptableObject reference or DisplayName")]
    public string Id => _id;

    [SerializeField, FormerlySerializedAs("numericID"), FormerlySerializedAs("NumericID")]
    private int _numericID;
    public int NumericID => _numericID;

    [Header("General")]
    [SerializeField, FormerlySerializedAs("displayName"), FormerlySerializedAs("DisplayName")]
    private string _displayName;
    public string DisplayName => _displayName;

    [SerializeField, FormerlySerializedAs("description"), FormerlySerializedAs("Description")]
    private string _description;
    public string Description => _description;

    [SerializeField, FormerlySerializedAs("mainQuest"), FormerlySerializedAs("MainQuest")]
    private bool _mainQuest;
    public bool MainQuest => _mainQuest;

    [SerializeField, FormerlySerializedAs("defaultPos"), FormerlySerializedAs("DefaultPos")]
    private Vector3 _defaultPos;
    public Vector3 DefaultPos => _defaultPos;
    public Transform QuestStartTeleport { get; set; } // This will be set runtime and not serialized


    [Header("Requirements")]
    [SerializeField, FormerlySerializedAs("levelRequirement"), FormerlySerializedAs("LevelRequirement")]
    private int _levelRequirement;
    public int LevelRequirement => _levelRequirement;

    [SerializeField, FormerlySerializedAs("questPrerequisites"), FormerlySerializedAs("QuestPrerequisites")]
    private QuestScriptableObject[] _questPrerequisites;
    public QuestScriptableObject[] QuestPrerequisites => _questPrerequisites;

    [Header("Steps")]
    [SerializeField, FormerlySerializedAs("questStepPrefabs")]
    private GameObject[] _questStepPrefabs;
    public GameObject[] QuestStepPrefabs => _questStepPrefabs;

    [Header("Rewards")]
    [SerializeField, FormerlySerializedAs("experienceReward"), FormerlySerializedAs("ExperienceReward")]
    private int _experienceReward;
    public int ExperienceReward => _experienceReward;

    [SerializeField, FormerlySerializedAs("abilityReward"), FormerlySerializedAs("AbilityReward")]
    private Abilities _abilityReward;
    public Abilities AbilityReward => _abilityReward;

    public override string ToString() => DisplayName;
    private void OnValidate()
    {
#if UNITY_EDITOR
        if(string.IsNullOrEmpty(_id))
        {
            _id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
