using System.Collections.Generic;
using UnityEngine;

public class DevToolsTeleportList : MonoBehaviour
{
    [SerializeField] private TeleportButton _teleportButtonPrefab;

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        List<ITeleportPoint> teleportPoints = new List<ITeleportPoint>();

        var rootObjects = gameObject.scene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            teleportPoints.AddRange(rootObject.GetComponentsInChildren<ITeleportPoint>());
        }

        foreach (ITeleportPoint teleportPoint in teleportPoints)
        {
            TeleportButton newButton = Instantiate(_teleportButtonPrefab, transform);
            newButton.Target = teleportPoint;
            newButton.gameObject.name = "TeleportButton " + teleportPoint.Name;
        }
    }
}
