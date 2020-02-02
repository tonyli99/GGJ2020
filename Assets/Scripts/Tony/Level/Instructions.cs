using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown || Input.GetButtonDown("Fire1") || Input.GetButton("Fire1_Player2"))
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
