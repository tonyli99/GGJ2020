using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPart : MonoBehaviour
{
    public Body body;

    public Limb testLim;

    public float pickupDelay;

    IEnumerator routine;
    private void Start()
    {
        routine = PickUP(pickupDelay);
        StartCoroutine(routine);
    }

    IEnumerator PickUP(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        body.ReplaceWith(testLim);
    }
}
