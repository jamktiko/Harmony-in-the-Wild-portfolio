using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class SplineCollider : MonoBehaviour
{
    [SerializeField] private SplineContainer _container;
    [SerializeField] private int _splineIndex = 0;
    [SerializeField] private float _height = 5.0f;
    [SerializeField] private int _subdivisions = 100;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _inverseDirection;

#if UNITY_EDITOR
    private static bool _isProcessing;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    [UnityEditor.MenuItem("GameObject/3D Object/Spline Collider", false, priority = -100)]
    private static void CreateObject()
    {
        if (_isProcessing)
            return;
        _isProcessing = true;

        GameObject[] selectedObjects = UnityEditor.Selection.gameObjects;

        UnityEditor.EditorApplication.delayCall += () =>
        {
            _isProcessing = false; // Reset flag
            foreach (var selection in selectedObjects)
            {
                if (selection is not GameObject)
                    return;

                GameObject go = new GameObject("SplineCollider");
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshCollider>();
                var splineCollider = go.AddComponent<SplineCollider>();
                splineCollider._container = selection.GetComponent<SplineContainer>();
                go.transform.SetParent((selection as GameObject).transform);
                go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                go.transform.localScale = Vector3.one;
                UnityEditor.EditorUtility.SetDirty(go);
            }
        };
    }
    private void OnValidate()
    {
        if (_container == null)
            return;

        if (_subdivisions < 4)
            _subdivisions = 4;

        if (_splineIndex < 0)
            _splineIndex = 0;
        if (_splineIndex >= _container.Splines.Count)
            _splineIndex = _container.Splines.Count - 1;

        if (_container == null)
            _container = GetComponentInParent<SplineContainer>();

        if (_container.Splines.Count == 0)
            Debug.LogError("SplineContainer has no splines", this);
    }
#endif

    private void Start()
    {
        _meshCollider = GetComponent<MeshCollider>();
        if (_meshCollider.sharedMesh == null)
            GenerateMesh();
    }

    [ContextMenu("Generate Collider")]
    public void GenerateMesh()
    {
        if (_container == null)
            return;

        Mesh mesh = new Mesh();
        mesh.name = "SplineCollider";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = GenerateVertices();
        if (_loop)
            mesh.triangles = GenerateTrianglesLoop();
        else
            mesh.triangles = GenerateTriangles();

        mesh.RecalculateNormals();

        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    private Vector3[] GenerateVertices()
    {
        Vector3 previousPosition = Vector3.zero;
        Vector3[] vertices = new Vector3[_subdivisions * 2];

        if (_splineIndex < 0)
            _splineIndex = 0;
        if (_splineIndex >= _container.Splines.Count)
            _splineIndex = _container.Splines.Count - 1;

        for (int i = 0; i < _subdivisions; i++)
        {
            float t = i / (float)_subdivisions;
            _container.Splines[_splineIndex].Evaluate(_inverseDirection ? 1f - t : t, out float3 position, out float3 tangent, out float3 upVector);
            Vector3 splinePosition = position;
            vertices[2 * i] = splinePosition;
            vertices[(2 * i) + 1] = splinePosition + Vector3.up * _height;
        }
        return vertices;
    }


    private int[] GenerateTriangles()
    {
        int[] triangles = new int[(_subdivisions - 1) * 6];
        for (int i = 0; i < _subdivisions - 1; i++)
        {
            triangles[6 * i] = 2 * i;
            triangles[(6 * i) + 1] = 2 * i + 1;
            triangles[(6 * i) + 2] = 2 * i + 2;
            triangles[(6 * i) + 3] = 2 * i + 1;
            triangles[(6 * i) + 4] = 2 * i + 3;
            triangles[(6 * i) + 5] = 2 * i + 2;
        }

        return triangles;
    }

    private int[] GenerateTrianglesLoop()
    {
        int[] triangles = new int[_subdivisions * 6];
        for (int i = 0; i < _subdivisions - 1; i++)
        {
            triangles[6 * i] = 2 * i;
            triangles[(6 * i) + 1] = 2 * i + 1;
            triangles[(6 * i) + 2] = 2 * i + 2;
            triangles[(6 * i) + 3] = 2 * i + 1;
            triangles[(6 * i) + 4] = 2 * i + 3;
            triangles[(6 * i) + 5] = 2 * i + 2;
        }

        // Fill the hole between the last and first vertices
        int lastIndex = (_subdivisions - 1) * 6;
        triangles[lastIndex] = 2 * (_subdivisions - 1);
        triangles[lastIndex + 1] = 2 * (_subdivisions - 1) + 1;
        triangles[lastIndex + 2] = 0;
        triangles[lastIndex + 3] = 2 * (_subdivisions - 1) + 1;
        triangles[lastIndex + 4] = 1;
        triangles[lastIndex + 5] = 0;

        return triangles;
    }

    private void OnDrawGizmosSelected()
    {
        if (_container == null)
            return;

        if (_subdivisions < 4)
            _subdivisions = 4;

        if (_splineIndex < 0)
            _splineIndex = 0;
        if (_splineIndex >= _container.Splines.Count)
            _splineIndex = _container.Splines.Count - 1;

        Vector3 previousPosition = Vector3.zero;
        for (int i = 0; i < _subdivisions; i++)
        {
            _container.Splines[_splineIndex].Evaluate(i / (float)_subdivisions, out float3 position, out float3 tangent, out float3 upVector);
            Vector3 splinePosition = transform.TransformPoint(position);

            if (i == 0)
            {
                previousPosition = splinePosition;
                continue;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(splinePosition, splinePosition + Vector3.up * _height);
            Gizmos.DrawLine(splinePosition, previousPosition);
            Gizmos.DrawLine(splinePosition + Vector3.up * _height, previousPosition + Vector3.up * _height);
            previousPosition = splinePosition;


            // Drawing arrows inwards to indicate the direction of the mesh
            Gizmos.color = Color.blue;
            Vector3 startPosition = splinePosition + Vector3.up * _height;
            Vector3 endPosition = startPosition + Quaternion.Euler(0, _inverseDirection ? 90 : -90, 0) * transform.TransformDirection(tangent);

            Debug.DrawLine(startPosition, endPosition, Color.blue, default, false);
        }
    }
}
