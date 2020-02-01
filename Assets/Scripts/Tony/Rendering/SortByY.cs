using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortByY : MonoBehaviour
{
    private SortingGroup sortingGroup;

    private void Awake()
    {
        sortingGroup = GetComponentInChildren<SortingGroup>();
    }

    private void Update()
    {
        sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
