using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    private bool _initialized;
    public static readonly Dictionary<QuestScriptableObject, Quest> QuestMap = new();
    private int _curID = 0;

    public static QuestManager Instance;

    [FormerlySerializedAs("playerManager")][SerializeField] private PlayerManager _playerManager;
    [FormerlySerializedAs("AbilityCycle")][SerializeField] private AbilityCycle _abilityCycle;
    [FormerlySerializedAs("mapQuestMarkersBW")][SerializeField] private Sprite[] _mapQuestMarkersBw;

    [FormerlySerializedAs("mapQuestMarkersColour")]
    [SerializeField]
    private Sprite[] _mapQuestMarkersColour;

    private int _currentPlayerLevel;
    private string _currentActiveQuest;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            Instance = this;

        }

        // initialize quest map
        //questMap = CreateQuestMap();
        _playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void Start()
    {
        // GetQuestById() might be called before Start, so separating the initialization logic into a separate method
        InitializeIfNeeded();
    }

    private void InitializeIfNeeded()
    {
        if (_initialized)
            return;
        _initialized = true;

        CreateQuestMap();
        CheckAllRequirements();

        _playerManager = FindObjectOfType<PlayerManager>();
        // broadcast the initial state of all quests on startup
        foreach (Quest quest in QuestMap.Values)
        {
            // initialize any loaded quest steps
            if (quest.State == QuestState.InProgress)
            {
                quest.InstantiateCurrentQuestStep(transform);
            }

            // broadcast the initial state of all quests
            GameEventsManager.instance.QuestEvents.QuestStateChange(quest);

            Debug.Log(quest.Info + ": state is set to " + quest.State);
        }

        if (SceneManager.GetActiveScene().name == "Overworld" && _abilityCycle == null)
        {
            _abilityCycle = FindObjectOfType<AbilityCycle>(); // In case Overworld is loaded in editor, find AbilityCycle
        }
    }

    public void SetQuestMarkers(Image[] mapQuestMarkers)
    {
        if (QuestMap != null)
        {
            foreach (KeyValuePair<QuestScriptableObject, Quest> quest in QuestMap)
            {
                if (quest.Value.Info.NumericID < mapQuestMarkers.Length)
                {
                    if (quest.Value.State == QuestState.RequirementsNotMet)
                        mapQuestMarkers[quest.Value.Info.NumericID].sprite = _mapQuestMarkersBw[quest.Value.Info.NumericID];
                    else
                        mapQuestMarkers[quest.Value.Info.NumericID].sprite = _mapQuestMarkersColour[quest.Value.Info.NumericID];
                }
            }
        }
    }

    private void PlayerLevelChange(int level)
    {
        _currentPlayerLevel = level;
    }

    public void CheckAllRequirements()
    {
        // loop through all quests
        foreach (Quest quest in QuestMap.Values)
        {
            // if meeting the requirements, switch over to CAN_START state
            if (quest.State == QuestState.RequirementsNotMet && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.Info, QuestState.CanStart);
            }
        }
    }
    private bool CheckRequirementsMet(Quest quest)
    {
        // start true and prove to be false
        bool meetsRequirements = true;

        /*if (currentPlayerLevel<quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }*/
        foreach (QuestScriptableObject prerequisiteQuestInfo in quest.Info.QuestPrerequisites)
        {
            if (GetQuest(prerequisiteQuestInfo).State != QuestState.Finished)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void ChangeQuestState(QuestScriptableObject id, QuestState state)
    {
        Debug.Log("Changed quest state with ID: " + id + " to: " + state.ToString());
        Quest quest = GetQuest(id);
        quest.State = state;
        //Debug.Log(quest.state.ToString());
        SaveManager.Instance.SaveGame(); // Force save game when quest state changes
        GameEventsManager.instance.QuestEvents.QuestStateChange(quest);
    }

    private void StartQuest(QuestScriptableObject questObject)
    {
        Quest quest = GetQuest(questObject);
        if (quest.State == QuestState.CanStart)
        {
            //Debug.Log("Start Quest: " + id);
            quest.InstantiateCurrentQuestStep(transform);
            ChangeQuestState(quest.Info, QuestState.InProgress);
            var tutorialQuestObject = QuestManager.GetQuestObject("Tutorial");
            if (questObject != tutorialQuestObject)
            {
                AbilityAcquired(quest.Info.AbilityReward);
                //Debug.Log("Ability unlocked: " + quest.info.abilityReward);
            }
        }
    }

    private void AdvanceQuest(QuestScriptableObject questObject)
    {
        Debug.Log("Advance Quest: " + questObject);
        Quest quest = GetQuest(questObject);

        // move on to the next step
        quest.MoveToNextStep();

        // if there are more steps, instantiate the next one
        if (quest.CurrentStepExists())
        {
            //Debug.Log("Quest " + id + " state advanced.");
            quest.InstantiateCurrentQuestStep(transform);
        }

        // if there are no more steps, it means the quest is ready to  be finished
        else
        {
            // if you are finishing a side quest, call the event that will enable showing the final quest UI for that side quest
            if (!quest.Info.MainQuest)
            {
                ChangeQuestState(quest.Info, QuestState.CanFinish);
                GameEventsManager.instance.QuestEvents.ReturnToSideQuestPoint(questObject);
            }

            // if you are finishing a main quest, instantly finish it since you won't need to return to any quest point for the actual finish
            else
            {
                Debug.Log("About to finish dungeon quest: " + questObject);
                ChangeQuestState(quest.Info, QuestState.CanFinish);
                //GameEventsManager.instance.questEvents.FinishQuest(id);
            }
        }
    }

    private void FinishQuest(QuestScriptableObject questObject)
    {
        //Debug.Log("Finished quest with ID: "+ id);
        Quest quest = GetQuest(questObject);
        ClaimRewards(quest);
        ChangeQuestState(quest.Info, QuestState.Finished);
        AudioManager.Instance.PlaySound(AudioName.ActionQuestCompleted, transform);
        GameEventsManager.instance.QuestEvents.HideQuestUI();
        QuestCompletedUI.Instance.ShowUI(questObject);
        ResetActiveQuest();
        CheckAllRequirements();
    }

    private void ClaimRewards(Quest quest)
    {
        //Debug.Log("Quest " + quest.info.id + " has been completed.");

        GameEventsManager.instance.PlayerEvents.ExperienceGained(quest.Info.ExperienceReward);

        if (quest.Info.AbilityReward != 0)
        {
            AbilityAcquired(quest.Info.AbilityReward);
            //StartCoroutine(AbilityCycle.MakeList());
        }
    }

    private void QuestStepStateChange(QuestScriptableObject id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuest(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.State);
    }

    private void SetActiveQuest(string id)
    {
        _currentActiveQuest = id;
    }

    private void ResetActiveQuest()
    {
        _currentActiveQuest = "";
    }

    public string GetActiveQuest()
    {
        return _currentActiveQuest;
    }

    public void CreateQuestMap()
    {
        Debug.Log("Reset quest map");
        QuestManager.QuestMap.Clear();

        // Deleting all child objects
        foreach (Transform questStep in transform)
        {
            Destroy(questStep.gameObject);
        }

        // load all QuestInfoSOs in path Assets/Resources/Quests
        QuestScriptableObject[] allQuests = Resources.LoadAll<QuestScriptableObject>("Quests");

        // load loaded data from Save Manager
        List<string> loadedQuestData = SaveManager.Instance.GetLoadedQuestData("quest");

        int currentQuestSoIndex = 0;

        foreach (QuestScriptableObject questInfo in allQuests)
        {
            if (QuestMap.TryGetValue(questInfo, out Quest quest))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo + " & " + quest.Info);
            }

            if (loadedQuestData.Count > 0)
            {
                QuestMap.Add(questInfo, LoadQuest(questInfo, loadedQuestData[currentQuestSoIndex]));
            }
            else
            {
                QuestMap.Add(questInfo, LoadQuest(questInfo, null));
            }

            currentQuestSoIndex++;
        }
    }

    [Obsolete("Use GetQuest using QuestScriptableObject instead")]
    public Quest GetQuest(string id)
    {
        InitializeIfNeeded();
        Quest quest = QuestMap.Where(o => o.Key.Id == id).FirstOrDefault().Value;

        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }

        return quest;
    }

    internal Quest GetQuest(QuestScriptableObject questObject)
    {
        InitializeIfNeeded();

        Quest quest = QuestMap[questObject];

        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + questObject);
        }

        return quest;
    }

    internal static QuestScriptableObject GetQuestObject(string id)
    {
#pragma warning disable CS0618 // Type or member is obsolete. This is a workaround to get the QuestScriptableObject from the ID
        return QuestMap.Where(o => o.Key.Id == id).FirstOrDefault().Key;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    [Obsolete("Use GetQuest using QuestScriptableObject instead")]
    internal Quest GetQuestById(string id)
    {
        var questObject = GetQuestObject(id);
        return GetQuest(questObject);
    }

    public QuestState CheckQuestState(QuestScriptableObject questObject)
    {
        if (questObject == null)
            throw new ArgumentNullException("questObject has to be defined");

        Quest quest = GetQuest(questObject);

        return quest.State;
    }

    private void AbilityAcquired(Abilities ability)
    {
        AbilityManager.Instance.UnlockAbility(ability);

        if (ability == Abilities.GhostSpeaking)
        {
            ActivateGhostSpeak();
        }
    }
    public void ActivateGhostSpeak()
    {
        GameEventsManager.instance.PlayerEvents.GhostSpeakActivated();
    }
    public List<string> CollectQuestDataForSaving()
    {
        List<string> allQuestData = new List<string>();
        int i = 0;

        foreach (Quest quest in QuestMap.Values)
        {
            string savedQuestData = GetSerializedQuestData(quest);
            allQuestData.Add(savedQuestData);

            i++;
        }
        return allQuestData;
    }

    private string GetSerializedQuestData(Quest quest)
    {
        string serializedData = "";

        try
        {
            QuestData questData = quest.GetQuestData();
            serializedData = JsonUtility.ToJson(questData);
        }

        catch (System.Exception e)
        {
            Debug.LogError("Failed to save quest with id " + quest.Info + ": " + e);
        }

        return serializedData;
    }

    private Quest LoadQuest(QuestScriptableObject questInfo, string serializedData)
    {
        Quest quest = null;

        try
        {
            QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
            quest = new Quest(questInfo, questData.State, questData.QuestStepIndex, questData.QuestStepStates);
        }

        catch
        {
            quest = new Quest(questInfo);
        }

        return quest;
    }

    private void SubscribeToEvents()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest += StartQuest;
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.QuestEvents.OnFinishQuest += FinishQuest;

        GameEventsManager.instance.QuestEvents.OnQuestStepStateChange += QuestStepStateChange;

        GameEventsManager.instance.PlayerEvents.OnExperienceGained += PlayerLevelChange;
        GameEventsManager.instance.PlayerEvents.OnAbilityAcquired += AbilityAcquired;

        GameEventsManager.instance.QuestEvents.OnChangeActiveQuest += SetActiveQuest;
    }

    private void UnsubscribeFromEvents()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest -= StartQuest;
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= FinishQuest;

        GameEventsManager.instance.QuestEvents.OnQuestStepStateChange -= QuestStepStateChange;

        GameEventsManager.instance.PlayerEvents.OnExperienceGained -= PlayerLevelChange;
        GameEventsManager.instance.PlayerEvents.OnAbilityAcquired -= AbilityAcquired;

        GameEventsManager.instance.QuestEvents.OnChangeActiveQuest -= SetActiveQuest;
    }
    public void RequestFinishQuest(QuestScriptableObject questObject) // For some reason the event doesn't trigger reliably so as a workaround to ensure dungeons finish, this is called.
    {
        FinishQuest(questObject);
    }
}
