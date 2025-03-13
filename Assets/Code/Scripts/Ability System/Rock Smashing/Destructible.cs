using System.Collections;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Destructible : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool _needsToBeFrozen;
    
    [Description("Should be true if rock needs minigame, ONLY ENABLE FOR SMASHING! ROCKS")]
    [HideInInspector]public bool IsQuestRock;
    
    [HideInInspector]public float MinRequiredForce;
    [HideInInspector]public float MaxRequiredForce;

    [Header("Needed References")]
    [SerializeField] private GameObject _destroyedVersion;

    private bool _destroyCurrentObject;
    private bool _smashingInProgress;

    private void OnEnable()
    {
        _smashingInProgress = false;
        EditorUtility.SetDirty(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && AbilityManager.Instance.AbilityStatuses.TryGetValue(Abilities.RockDestroying, out bool isUnlocked) && isUnlocked && !_smashingInProgress)
        {
            if (_needsToBeFrozen)
            {
                if (gameObject.GetComponent<Freezable>().IsFrozen)
                {
                    AudioManager.Instance.PlaySound(AudioName.AbilityRockSmashing, transform);
                    _smashingInProgress = true;
                    CreateDestroyedVersion();
                }

                else
                {
                    Debug.Log(gameObject.name + " needs to be frozen first.");
                }
            }

            else if (!IsQuestRock)
            {
                AudioManager.Instance.PlaySound(AudioName.AbilityRockSmashing, transform);
                _smashingInProgress = true;
                _destroyCurrentObject = true;
                CreateDestroyedVersion();
            }
        }
    }

    private void CreateDestroyedVersion()
    {
        if (_destroyCurrentObject)
        {
            Instantiate(_destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        else
        {
            Instantiate(_destroyedVersion, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    public void DestroyQuestRock()
    {
        AudioManager.Instance.PlaySound(AudioName.AbilityRockSmashing, transform);
        _smashingInProgress = true;
        _destroyCurrentObject = true;
        CreateDestroyedVersion();
    }
}
