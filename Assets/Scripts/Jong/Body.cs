﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    Dictionary<Limb.PartType, Limb> bodyParts;
    Dictionary<Limb.PartType, Transform> partMountPoint;
    //Dictionary<Limb.PartType, float> maxDecayTime;
    public enum HostType { Human, Zombie };
    public HostType hostType;

    public Transform headPoint;
    public Transform leftArmPoint;
    public Transform rightArmPoint;
    public Transform leftLegPoint;
    public Transform rightLegPoint;

    //public float headMaxDecayTime;
    //public float leftArmMaxDecayTime;
    //public float rightArmMaxDecayTime;
    //public float leftLegMaxDecayTime;
    //public float rightLegMaxDecayTime;

    public Transform bodyTranform;
    public CharacterActor ca;
    private void Awake()
    {
        bodyParts = new Dictionary<Limb.PartType, Limb>(5);
        partMountPoint = new Dictionary<Limb.PartType, Transform>(5);
        partMountPoint[Limb.PartType.Head] = headPoint;
        partMountPoint[Limb.PartType.LeftArm] = leftArmPoint;
        partMountPoint[Limb.PartType.RightArm] = rightArmPoint;
        partMountPoint[Limb.PartType.LeftLeg] = leftLegPoint;
        partMountPoint[Limb.PartType.RightLeg] = rightLegPoint;

        //maxDecayTime = new Dictionary<Limb.PartType, float>(5);
        //maxDecayTime[Limb.PartType.Head] = headMaxDecayTime;
        //maxDecayTime[Limb.PartType.LeftArm] = leftArmMaxDecayTime;
        //maxDecayTime[Limb.PartType.RightArm] = rightArmMaxDecayTime;
        //maxDecayTime[Limb.PartType.LeftLeg] = leftLegMaxDecayTime;
        //maxDecayTime[Limb.PartType.RightLeg] = rightLegMaxDecayTime;
    }

    private void Start()
    {
        //StartCoroutine(WaitAndDrop());
    }

    IEnumerator WaitAndDrop()
    {
        yield return new WaitForSeconds(1);
        //ca.SetAnimatorStatus(false);
        DropAll();

        //yield return new WaitForSeconds(.1f);
        //ca.SetAnimatorStatus(true);

    }

    // replace or attach body part
    public void ReplaceWith(Limb newPart)
    {

        if (newPart == null)
        {
            return;
        }
        //Debug.Log("Incoming:" + newPart.partType);
        DropPart(newPart.partType);
        bodyParts[newPart.partType] = newPart;
        bodyParts[newPart.partType].Reattach(this);
    }

    public Transform GetMountPoint(Limb.PartType t)
    {
        return partMountPoint[t];
    }

    public void DropPart(Limb.PartType pt)
    {
        if (bodyParts.ContainsKey(pt) && bodyParts[pt] != null)
        {
            Limb droppingPart = bodyParts[pt];
            bodyParts[pt] = null;
            droppingPart.Toss();
        }
    }

    public void DropAll()
    {
        //bodyParts[Limb.PartType.Head].Toss();
        //return;
        foreach (Limb limb in bodyParts.Values)
        {
            if (limb != null)
            {
                limb.Toss();
            }
        }
    }

    public void LostPart(Limb.PartType pt)
    {
        DropPart(pt);
        if (bodyParts[Limb.PartType.LeftLeg] == null && bodyParts[Limb.PartType.RightArm] == null)
        {
            StartCoroutine("DropDeadAnimation");
        }
    }

    IEnumerator DropDeadAnimation()
    {
        for (float t = 0; t <= 1; t += Time.deltaTime * 2.0f)
        {
            bodyTranform.position = Vector3.Lerp(bodyTranform.position, transform.position, t);
            yield return null;
        }
    }

    public void TakeDamage(float damange)
    {
        foreach (Limb part in bodyParts.Values)
        {
            if (part != null)
            {
                part.AddDamage(damange);
            }
        }
    }

    public void TakePotion(float life)
    {
        TakeDamage(-life);
    }
}
