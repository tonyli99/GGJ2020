using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActor : MonoBehaviour
{
    [Header("Speeds")]
    public float verticalSpeed = 3;
    public float horizontalSpeed = 5;
    public float sprintSpeed = 10;
    public float sprintDuration = 2;
    public float sprintCooldownDuration = 5;

    [Header("Animator")]
    public string idleParameter;
    public string runParameter;
    public string[] attackParameters;

    [Header("Attack")]
    public GameObject attackCollider;

    public Vector2 movement { get; set; }

    private CharacterController2D controller;
    private int currentAttackIndex;
    private float nextSprintTime;

    public enum State { Idle, Sprinting, Attacking, Dead }
    private State state = State.Idle;

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                controller.movement = new Vector2(movement.x * horizontalSpeed, movement.y * verticalSpeed);
                break;
            case State.Sprinting:
                controller.movement = new Vector2(movement.x * sprintSpeed, movement.y * verticalSpeed);
                break;
            default:
                controller.movement = Vector2.zero;
                break;
        }
    }

    public void Sprint()
    {
        if (Time.time < nextSprintTime) return;
        nextSprintTime = Time.time + sprintCooldownDuration;
        state = State.Sprinting;
        Invoke("StopSprinting", sprintDuration);
    }

    public void StopSprinting()
    {
        if (state == State.Sprinting) state = State.Idle;
    }

    public void Attack()
    {
        if (state != State.Idle) return;
        state = State.Attacking;
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        yield return null;

        state = State.Idle;
    }
}
