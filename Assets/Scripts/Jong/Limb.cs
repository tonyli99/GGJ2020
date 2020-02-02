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
        if (hostBody != null)
        //if (hostBody != null && hostBody.hostType == Body.HostType.Zombie)
        {
            hostBody.ReplaceWith(this);
            //Debug.Log("Limb:" + partType);
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
        yield return null;
        Vector3 dropPos = oldBody.position;
        //transform.localScale = Vector3.one;
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

    IEnumerator TossingAnimation()
    {
        yield return null;
        float yAxixAccel = -10;
        Vector3 vel = new Vector3((Random.value - .5f) * 8, Random.value * 8, 0);
        Debug.Log("vel.y = " + vel.y);
        float dropPosY = oldBody.position.y;
        transform.localScale = Vector3.one;
        Quaternion q = Quaternion.Euler(0, 0, 90 * (Random.value > .5f ? 1 : -1));

        float travelTime = Mathf.Abs(vel.y / yAxixAccel);
        Debug.Log("travelTime1 = " + travelTime);
        float travelHeight = vel.y * travelTime + .5f * yAxixAccel * travelTime * travelTime; //V * t + 1/2 * a * t^2
        float totalDistance = Mathf.Abs(travelHeight - dropPosY) + (vel.y * .5f);

        travelTime += Mathf.Sqrt(2 * totalDistance / -yAxixAccel);

        Debug.Log("travelHeight = " + travelHeight);
        Debug.Log("totalDistance = " + totalDistance);
        Debug.Log("travelTime = " + travelTime);

        Quaternion initRot = transform.rotation;
        for (float t = 0; t <= travelTime; t += Time.deltaTime * 1.0f)
        {
            transform.position = transform.position + vel * Time.deltaTime;
            vel.y += Time.deltaTime * yAxixAccel;
            
            float et = t / travelTime;
            Debug.Log("t = " + t);
            Debug.Log("et = " + et);
            transform.rotation = Quaternion.Lerp(initRot, q, et);
            foreach (Transform ct in transform)
            {
                ct.localRotation = Quaternion.Lerp(ct.localRotation, Quaternion.identity, et);
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

        StartCoroutine(DroppingAnimation());
    }

    Vector3 partPos;
    public void Toss()
    {
        oldBody = hostBody.transform;
        partPos = transform.position;
        hostBody = null;
        transform.parent = null;

        StartCoroutine(TossingAnimation());
    }

    public void Reattach(Body body)
    {
        col.enabled = false;
        hostBody = body;

        if (hostBody.hostType == Body.HostType.Zombie)
        {
            Transform mountPoint = hostBody.GetMountPoint(partType);
            transform.parent = mountPoint;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            currentColor = Color.green;
            Decay();
        }


    }

    public void AddDamage(float damage)
    {
        currentTime += damage;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);
    }

}
