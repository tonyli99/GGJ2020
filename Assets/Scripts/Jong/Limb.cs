﻿using System.Collections;
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

    float groundY;
    List<SpriteRenderer> spriteList;
    private Shader originalShader;
    private Shader flashShader;
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
        originalShader = spriteList[0].material.shader;
        flashShader = Shader.Find("GUI/Text Shader");

        hostBody = GetComponentInParent<Body>();
        if (hostBody != null)
        //if (hostBody != null && hostBody.hostType == Body.HostType.Zombie)
        {
            hostBody.ReplaceWith(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hostBody == null)
        {
            decayMultiplier = 0.05f;
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
                groundY = hostBody.transform.position.y;
                hostBody.LostPart(partType);

            }
            currentTime = maxTime;
        }
        Color c = currentColor;
        if (touched)
        {
            c = touchedColor * (1 - (Time.time - (int)(Time.time)) * .5f);
        }
        else
        {
            c = currentColor * (1 - currentTime / maxTime);
        }
        c.a = 1;
        foreach (var sr in spriteList)
        {
            sr.color = c;
        }
    }

    IEnumerator TossingAnimation()
    {
        yield return null;
        float yAxixAccel = -10;
        Vector3 vel = new Vector3((Random.value - .5f) * 8, Random.value * 8, 0);
        float dropPosY = groundY;
        transform.localScale = Vector3.one;
        Quaternion q = Quaternion.Euler(0, 0, 90 * (Random.value > .5f ? 1 : -1));

        float travelTime = Mathf.Abs(vel.y / yAxixAccel);
        float travelHeight = vel.y * travelTime + .5f * yAxixAccel * travelTime * travelTime; //V * t + 1/2 * a * t^2
        float totalDistance = Mathf.Abs(travelHeight - dropPosY) - (vel.y * .25f);

        travelTime += Mathf.Sqrt(2 * totalDistance / -yAxixAccel);

        Quaternion initRot = transform.rotation;
        for (float t = 0; t <= travelTime; t += Time.deltaTime * 1.0f)
        {
            if (transform.position.y >= groundY)
            {
                transform.position = transform.position + vel * Time.deltaTime;
                vel.y += Time.deltaTime * yAxixAccel;
            }

            float et = t / travelTime;
            transform.rotation = Quaternion.Lerp(initRot, q, et);
            foreach (Transform ct in transform)
            {
                ct.localRotation = Quaternion.Lerp(ct.localRotation, Quaternion.identity, et);
            }
            yield return null;
        }

        currentColor = Color.grey;
        Decay();

        if (currentTime < maxTime && Random.value >= .3f)
        {
            col.enabled = true;
        }
    }

    public void Drop()
    {
        groundY = hostBody.transform.position.y;
        hostBody = null;
        transform.parent = null;

        StartCoroutine(TossingAnimation());
    }

    Vector3 partPos;
    public void Toss()
    {
        groundY = hostBody.transform.position.y;
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

    private bool touched = false;
    private Color touchedColor = Color.yellow;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        touched = true;
        SetShader(flashShader);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touched = false;
        SetShader(originalShader);
    }

    private void SetShader(Shader shader)
    {
        foreach (var sr in spriteList)
        {
            sr.material.shader = shader;
        }
    }
}
