using UnityEngine;

internal interface ITeleportPoint
{
    internal string Name { get; }
    internal Vector3 Position { get; }
    internal Quaternion Rotation { get; }
}