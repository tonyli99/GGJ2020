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

    Transform oldBody;
    List<SpriteRenderer> spriteList;
    Collider2D col;
    Color currentColor;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteList = new List<SpriteRenderer>();
        spriteList.Add(GetComponent<SpriteRenderer>());
        spriteList.AddRange(GetComponentsInChildren<SpriteRenderer>());
        currentColor = Color.grey;
    }
    // Start is called before the first frame update
    void Start()
    {
        hostBody = GetComponentInParent<Body>();
        if (hostBody != null && hostBody.hostType == Body.HostType.Zombie)
        {
            hostBody.ReplaceWith(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hostBody == null)
        {
            decayMultiplier = 1f;
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
                oldBody = hostBody.transform;
                //ground.x = ground.x + Random.value * 0.2f;
                hostBody.LostPart(partType);

            }
            currentTime = maxTime;
        }

        Color c = currentColor * (1 - currentTime / maxTime);
        c.a = 1;
        foreach (var sr in spriteList)
        {
            sr.color = c;
        }
    }

    IEnumerator DroppingAnimation()
    {
        Vector3 dropPos =  oldBody.position;
        transform.localScale = Vector3.one;
        Quaternion q = Quaternion.Euler(0, 0, 90 * (Random.value > .5f ? 1 : -1));
        for (float t = 0; t <= 1; t += Time.deltaTime * 2.0f)
        {
            transform.position = Vector3.Lerp(transform.position, dropPos, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, t);
            foreach (Transform ct in transform)
            {
                ct.localRotation = Quaternion.Lerp(ct.localRotation, Quaternion.identity, t);
            }
            yield return null;
        }

        currentColor = Color.grey;
        Decay();

        if (currentTime < maxTime)
        {
            col.enabled = true;
        }
    }

    public void Drop()
    {
        oldBody = hostBody.transform;
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
        currentColor = Color.green;

        col.enabled = false;

        Decay();
    }

    public void AddDamage(float damage)
    {
        currentTime += damage;
    }

}
