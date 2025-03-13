using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevToolAbilityList : MonoBehaviour
{
    [SerializeField] private AbilityToggling abilityTogglingPrefab;

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var ability in (Abilities[])System.Enum.GetValues(typeof(Abilities)))
        {
            // Skipping "None"
            if (ability == Abilities.None)
            {
                continue;
            }

            var abilityToggling = Instantiate(abilityTogglingPrefab, transform);
            abilityToggling.transform.SetParent(transform);
            abilityToggling.gameObject.name = "Ability " + ability.ToString();
            abilityToggling.Ability = ability;
            abilityToggling.label.text = ability.ToString();
            abilityToggling.toggle.isOn = AbilityManager.Instance.IsAbilityUnlocked(ability);
        }
    }
}
