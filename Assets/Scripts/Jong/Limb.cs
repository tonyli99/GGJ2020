using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    public enum PartType { Head, LeftArm, RightArm, LeftLeg, RightLeg };
    public PartType partType;
    public float decayMultiplier;
    public float maxTime;
    private float currentTime;

    public Body hostBody;

    Vector3 ground;
    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (hostBody != null && hostBody.hostType == Body.HostType.Zombie)
        {
            hostBody.ReplaceWith(this);
        }
        //Color c = sr.color;
        //c = Color.green;
        //sr.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        if (hostBody == null)
        {
            decayMultiplier = 1;
            Decay();
        }
        else if (hostBody.hostType == Body.HostType.Zombie)
        {
            decayMultiplier = 1;
            Decay();
        }
    }

    //Decay body part
    //change color
    void Decay()
    {
        currentTime += Time.deltaTime * decayMultiplier;
        if (currentTime >= maxTime)
        {
            if (hostBody != null)
            {
                ground = hostBody.transform.position;
                ground.x = ground.x + Random.value * 0.2f;
                hostBody.DropPart(partType);
            }
            currentTime = maxTime;
        }

        Color c = sr.color;
        c.g = 1 - currentTime / maxTime;
        sr.color = c;
    }

    IEnumerator DroppingAnimation()
    {
        for (float t = 0; t <= .5f; t += Time.deltaTime)
        {
            Vector3 pos = transform.position;
            transform.position = Vector3.Lerp(transform.position, ground, t);
            yield return null;
        }
    }

    public void Drop()
    {
        hostBody = null;
        transform.parent = null;

        StartCoroutine("DroppingAnimation");
    }

    public void Reattach(Body body)
    {
        hostBody = body;
        Transform mountPoint = hostBody.GetMountPoint(partType);
        transform.parent = mountPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        Color c = sr.color;
        c = Color.green;
        sr.color = c;
        Decay();
    }
}
