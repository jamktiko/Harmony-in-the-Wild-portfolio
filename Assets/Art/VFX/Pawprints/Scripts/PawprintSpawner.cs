using UnityEngine;
using System.Collections.Generic;

/*
    Pawprint Spawner Script

    Purpose:
    - Spawns a pawprint decal at the location of a trigger collider when it enters a collision.
    - Uses raycasting to ensure the pawprint is placed on the terrain.
    - Object pooling is used to reduce the number of instantiations and destroy calls.
*/

public class PawprintSpawner : MonoBehaviour
{
    public Pawprint pawprintPrefab; // Prefab for the pawprint decal
    public float decalLifetime = 60f; // Time before the decal is destroyed
    public int maxPawprints = 10; // Maximum number of pawprints that can be active at once
    public List<Transform> paws;
    public Queue<Pawprint> activePawprints = new Queue<Pawprint>();
    public Queue<Pawprint> inactivePawprints = new Queue<Pawprint>();

    public LayerMask groundLayerMask;
    private GameObject _pawprintContainer;

    private void Start()
    {
        if(_pawprintContainer == null)
        {
            _pawprintContainer = new GameObject();
            _pawprintContainer.name = "Pawprints";
            _pawprintContainer.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        // Preload pawprints
        for (int i = 0; i < maxPawprints; i++)
        {
            var pawprint = Instantiate(pawprintPrefab, Vector3.zero, pawprintPrefab.transform.rotation);
            inactivePawprints.Enqueue(pawprint);
            pawprint.gameObject.SetActive(false);
            pawprint.gameObject.name = "PP " + i;
            pawprint.transform.SetParent(_pawprintContainer.transform);
        }

        foreach (var paw in paws)
        {
            var trigger = paw.gameObject.AddComponent<PawprintTrigger>();
            trigger.layerMask = groundLayerMask;
            trigger.onTrigger += OnPawTrigger;
        }
    }

    private void Update()
    {
        // Removing pawprints
        while (activePawprints.Count > 0 && Time.time - activePawprints.Peek().spawnTime > decalLifetime)
        {
            Pawprint print = activePawprints.Dequeue();
            print.gameObject.SetActive(false);
            inactivePawprints.Enqueue(print);
        }
    }

    private void OnPawTrigger(PawprintTrigger trigger)
    {
        TrySpawnPawprint(trigger.transform);
    }

    private void TrySpawnPawprint(Transform paw)
    {
        if (!paw.gameObject.activeInHierarchy)
            return;

        // Raycast downward to adjust placement on the ground
        RaycastHit hit;
        if (Physics.Raycast(paw.position + Vector3.up * 0.5f, Vector3.down, out hit, 1f, groundLayerMask))
        {
            var position = hit.point;

            // Taking pawprint from the inactive queue
            if (!inactivePawprints.TryDequeue(out Pawprint print))
            {
                // Reusing pawprint if it is null
                Debug.Log("Running out of pawprints!, reusing oldest active");
                activePawprints.TryDequeue(out print);
            }

            // Setting pawprint position and rotation and activating it
            if (print != null)
            {
                print.spawnTime = Time.time;
                print.fadeTime = decalLifetime;
                print.transform.SetPositionAndRotation(paw.transform.position, paw.transform.rotation * Quaternion.Euler(90, 0, 0));
                print.gameObject.SetActive(true);
                // Adding pawprint to the active queue
                activePawprints.Enqueue(print);
            }
        }
    }
}