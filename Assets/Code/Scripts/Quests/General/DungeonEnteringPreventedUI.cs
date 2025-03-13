using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonEnteringPreventedUI : MonoBehaviour
{
    [FormerlySerializedAs("requirementText")]
    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI _requirementText;

    private void Update()
    {
        if (PlayerInputHandler.Instance.CloseUIInput.WasPressedThisFrame())
        {
            CloseView();
        }
    }

    public void SetUIContent(Quest dungeon)
    {
        List<QuestScriptableObject> prerequisites = new List<QuestScriptableObject>();
        _requirementText.text = "";

        Debug.Log(dungeon.Info.DisplayName + " has " + dungeon.Info.QuestPrerequisites.Length + " quest requirements.");

        foreach (QuestScriptableObject requirement in dungeon.Info.QuestPrerequisites)
        {
            QuestState requirementState = QuestManager.Instance.CheckQuestState(requirement);

            if (requirementState != QuestState.Finished)
            {
                prerequisites.Add(requirement);
            }
        }

        foreach (QuestScriptableObject requirement in prerequisites)
        {
            _requirementText.text = _requirementText.text + requirement.DisplayName + "\n \n";
        }
    }

    private void CloseView()
    {
        gameObject.SetActive(false);
    }
}
