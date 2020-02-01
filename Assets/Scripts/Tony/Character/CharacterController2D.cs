using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public Vector2 movement { get; set; }

    public Limb touchingLimb;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var newPosition = rb.position + movement * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
        if (movement.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        touchingLimb = collision.gameObject.GetComponent<Limb>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Limb limb = collision.gameObject.GetComponent<Limb>();
        if (limb == touchingLimb)
        {
            touchingLimb = null;
        }
    }
}
