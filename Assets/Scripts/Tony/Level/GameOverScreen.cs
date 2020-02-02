using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    private static GameOverScreen instance;

    public GameObject canvas;
    public Button restartButton;

    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void GameOver()
    {
        instance.StartCoroutine(instance.GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1);
        foreach (var player in FindObjectsOfType<PlayerInput>())
        {
            Destroy(player.gameObject);
        }
        instance.canvas.SetActive(true);
        yield return new WaitForSeconds(1);
        EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
    }

}
