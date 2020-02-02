using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDestroy : MonoBehaviour
{
    public float chanceDestroy = 0.2f;

    private void Awake()
    {
        if (Random.value  < chanceDestroy)
        {
            Destroy(gameObject);
        }
    }
}
