using UnityEngine;

public class RockDestroying : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.RockDestroying, this);
    }

    public void Activate()
    {
        Debug.Log("RockDestroying activated");
    }

    public void Deactivate()
    {
        Debug.Log("RockDestroying deactivated");
    }
}
