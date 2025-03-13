using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePlayerIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _foxIcon;
    [SerializeField] private GameObject _arcticFoxIcon;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        MapOpen.OnMapOpen += OnMapOpen;

    }

    private void OnMapOpen(bool isMapOpen)
    {
        Debug.Log("Map status: " +  isMapOpen);
       _foxIcon.SetActive(isMapOpen);
        _arcticFoxIcon.SetActive(isMapOpen);
    }

    private void OnDisable()
    {
        MapOpen.OnMapOpen -= OnMapOpen;
    }
}
