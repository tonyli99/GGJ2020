using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string sprint = "Fire1";

    private CharacterActor actor;

    private void Awake()
    {
        actor = GetComponent<CharacterActor>();
    }

    private void Update()
    {
        actor.movement = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        if (Input.GetButtonDown(sprint)) actor.Sprint();
    }

}
