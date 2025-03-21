using UnityEngine;
using UnityEngine.Serialization;

public class GetPlayerPosition : MonoBehaviour
{
    [FormerlySerializedAs("fox")] [SerializeField]
    private Transform _fox;

    private void FixedUpdate()
    {
        transform.SetLocalPositionAndRotation(new Vector3(_fox.position.x, 0, _fox.position.z), Quaternion.Euler(-90, 0, _fox.eulerAngles.y));
    }
}
