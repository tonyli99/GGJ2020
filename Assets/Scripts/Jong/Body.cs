using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    Dictionary<Limb.PartType, Limb> bodyParts;
    Dictionary<Limb.PartType, Transform> partMountPoint;
    public enum HostType { Human, Zombie };
    public HostType hostType;

    public Transform headPoint;
    public Transform leftArmPoint;
    public Transform rightArmPoint;
    public Transform leftLegPoint;
    public Transform rightLegPoint;

    public Transform bodyTranform;

    private void Awake()
    {
        bodyParts = new Dictionary<Limb.PartType, Limb>(5);
        partMountPoint = new Dictionary<Limb.PartType, Transform>(5);
        partMountPoint[Limb.PartType.Head] = headPoint;
        partMountPoint[Limb.PartType.LeftArm] = leftArmPoint;
        partMountPoint[Limb.PartType.RightArm] = rightArmPoint;
        partMountPoint[Limb.PartType.LeftLeg] = leftLegPoint;
        partMountPoint[Limb.PartType.RightLeg] = rightLegPoint;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // replace or attach body part
    public void ReplaceWith(Limb newPart)
    {
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
            droppingPart.Drop();
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
}
