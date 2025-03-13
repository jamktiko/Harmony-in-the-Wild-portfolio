using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class DevToolButtons : MonoBehaviour
{
#if DEBUG
    [FormerlySerializedAs("pageParent")] [SerializeField] private Transform _pageParent;
    [SerializeField] private int _defaultPage = 0;

    private void Start()
    {
        ToggleVisiblePage(_defaultPage);
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.DebugDevToolsInput.WasPressedThisFrame())
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);
        }
    }

    public void ToggleVisiblePage(int pageIndex)
    {
        for (int i = 0; i < _pageParent.childCount; i++)
        {
            _pageParent.GetChild(i).gameObject.SetActive(i == pageIndex);
        }
    }

    public void SaveAbilityChanges()
    {
        SaveManager.Instance.SaveGame();
    }

    public void ResetAbilities()
    {

        foreach (Abilities abilities in AbilityManager.Instance.AbilityStatuses.Keys)
        {
            AbilityManager.Instance.AbilityStatuses[abilities] = false;
        }

        SaveManager.Instance.SaveGame();
    }

    public void DeleteSaveFile()
    {
        File.Delete(Application.persistentDataPath + "/GameData.json");
        Debug.LogError("The save file has been deleted. Please restart the game to avoid any errors.");
    }
#endif
}
