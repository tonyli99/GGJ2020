using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public string tagToDamage;
    public float attackDamage = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(tagToDamage)) return;
        //Debug.Log(name + " damaged " + collision, collision);
        collision.BroadcastMessage("TakeDamage", attackDamage);
    }
}
