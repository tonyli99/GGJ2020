using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : MonoBehaviour
{
    private CharacterActor actor;

    public enum State { Idle, Wander, Attack }
    [Header("Debug")]
    public State state = State.Idle;
    private float nextDirectionChange;
    public Vector2 movement;

    private void Awake()
    {
        actor = GetComponent<CharacterActor>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                actor.movement = movement;
                if (Time.time > nextDirectionChange)
                {
                    movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    nextDirectionChange = Time.time + 5;
                }
                break;
        }
    }

}
