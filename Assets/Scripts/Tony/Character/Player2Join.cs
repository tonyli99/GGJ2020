﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Join : MonoBehaviour
{
    public GameObject player2;
    public GameObject player2vcam;

    void Update()
    {
        if (Input.GetButtonDown("Fire1_Player2"))
        {
            player2.transform.position = FindObjectOfType<PlayerInput>().transform.position + 2 * Vector3.left;
            player2.SetActive(true);
            player2vcam.SetActive(true);
            Destroy(gameObject);
        }
    }
}
