using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewBlock : MonoBehaviour
{
    public GameObject blockPrefab;
    public float blockWidth = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Instantiate(blockPrefab, transform.parent.position + Vector3.right * blockWidth, Quaternion.identity);
        Destroy(gameObject);
    }
}
