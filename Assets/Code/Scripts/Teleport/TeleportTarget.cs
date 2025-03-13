using UnityEngine;

public class TeleportTarget : MonoBehaviour, ITeleportPoint
{
    string ITeleportPoint.Name => gameObject.name;
    Vector3 ITeleportPoint.Position => gameObject.transform.position;
    Quaternion ITeleportPoint.Rotation => gameObject.transform.rotation;
}
