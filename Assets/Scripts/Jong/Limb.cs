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
    List<SpriteRenderer> spriteList;
    private void Awake()
    {
        spriteList = new List<SpriteRenderer>();
        spriteList.Add(GetComponent<SpriteRenderer>());
        spriteList.AddRange(GetComponentsInChildren<SpriteRenderer>());

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
            decayMultiplier = .5f;
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
                hostBody.LostPart(partType);

            }
            currentTime = maxTime;
        }

        foreach (var sr in spriteList)
        {
            Color c = sr.color;
            c.g = 1 - currentTime / maxTime;
            sr.color = c;
        }
    }

    IEnumerator DroppingAnimation()
    {
        Quaternion q = Quaternion.Euler(0, 0, 90 * (Random.value > .5f ? 1 : -1));
        for (float t = 0; t <= 1; t += Time.deltaTime * 2.0f)
        {
            transform.position = Vector3.Lerp(transform.position, ground, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, t);
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

        foreach (var sr in spriteList)
        {
            Color c = sr.color;
            c = Color.green;
            sr.color = c;
        }
        Decay();
    }
}
