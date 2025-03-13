using UnityEngine;
using UnityEngine.UI;

public class AbilityToggling : MonoBehaviour
{
    [SerializeField] internal TMPro.TextMeshProUGUI label;
    [SerializeField] internal Toggle toggle;
    internal Abilities Ability { get; set; }

    public void ToggleAbility(bool value)
    {
        if (value)
        {
            AbilityManager.Instance.UnlockAbility(Ability);
        }
        else
        {
            AbilityManager.Instance.LockAbility(Ability);
        }
    }
}
