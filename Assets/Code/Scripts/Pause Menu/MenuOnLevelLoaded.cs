using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuOnLevelLoaded : MonoBehaviour
{
    [FormerlySerializedAs("pauseMenuPanel")] [SerializeField] private GameObject _pauseMenuPanel;

    private void OnEnable() => SceneManager.sceneLoaded += OnLevelLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnLevelLoaded;

    private void OnLevelLoaded(Scene arg0, LoadSceneMode arg1) => OnLevelLoaded(arg0.buildIndex);
    private void OnLevelLoaded(int level)
    {
        if (level == 1)
        {
            _pauseMenuPanel.SetActive(false);
        }
    }
}
