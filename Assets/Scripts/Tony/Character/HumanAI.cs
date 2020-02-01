using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : MonoBehaviour
{
    private CharacterActor actor;

    public float timeBetweenMovementChange = 5f;

    public enum State { Wander, Attack, Dead }
    [Header("Debug")]
    public State state = State.Wander;
    private float nextDirectionChange;
    public Vector2 movement;

    private VisionTrigger visionTrigger;

    private void Awake()
    {
        actor = GetComponent<CharacterActor>();
        visionTrigger = GetComponentInChildren<VisionTrigger>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Wander:
                if (visionTrigger.seesEnemy)
                {
                    if (Random.value < 0.5f)
                    {
                        movement = new Vector2(1, 0);
                        nextDirectionChange = Time.time + timeBetweenMovementChange;
                    }
                    else
                    {
                        actor.Attack();
                    }
                }
                else
                {
                    actor.movement = movement;
                    if (Time.time > nextDirectionChange)
                    {
                        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                        nextDirectionChange = Time.time + timeBetweenMovementChange;
                    }
                }
                break;
            case State.Attack:
                movement = Vector2.zero;
                if (actor.state == CharacterActor.State.Idle) state = State.Wander;
                break;
            case State.Dead:
                movement = Vector2.zero;
                break;
        }
    }

}
