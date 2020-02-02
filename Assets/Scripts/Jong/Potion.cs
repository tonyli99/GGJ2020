using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public float energy;
    public AudioClip drinkSound;

    private void Start()
    {
        transform.position += Vector3.up;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(DisappearAnim());
    }

    ///TODO: animation
    IEnumerator DisappearAnim()
    {
        AudioSource.PlayClipAtPoint(drinkSound, Camera.main.transform.position);
        GetComponent<Animator>().Play("drink");
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
