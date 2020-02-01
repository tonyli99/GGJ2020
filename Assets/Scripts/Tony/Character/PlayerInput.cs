using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string attack = "Fire1";
    public string sprint = "Fire2";
    public string pickup = "Pickup";

    private CharacterActor actor;

    private void Awake()
    {
        actor = GetComponent<CharacterActor>();
    }

    private void Update()
    {
        actor.movement = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        if (Input.GetButtonDown(sprint)) actor.Sprint();
        else if (Input.GetButtonDown(attack)) actor.Attack();
        else if (Input.GetButtonDown(pickup)) Debug.Log("Pick up");
    }

}
