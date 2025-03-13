using System;
using UnityEngine;

public class PawprintTrigger : MonoBehaviour
{
    internal LayerMask layerMask;
    internal event Action<PawprintTrigger> onTrigger;

    private void Start()
    {
        var c = gameObject.AddComponent<BoxCollider>();
        c.isTrigger = true;
        c.size = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checking if layer matches to layerMask
        if((layerMask.value & 1 << other.gameObject.layer) == 0)
            return;

        onTrigger?.Invoke(this);
    }
}