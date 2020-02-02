using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public float energy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(DisappearAnim());
    }

    ///TODO: animation
    IEnumerator DisappearAnim()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
