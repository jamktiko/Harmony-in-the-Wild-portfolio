using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineCollider))]
public class SplineColliderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Apply and Generate Mesh"))
        {
            SplineCollider splineCollider = (SplineCollider)target;
            splineCollider.GenerateMesh();
        }
    }
}
