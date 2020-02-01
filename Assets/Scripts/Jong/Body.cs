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
            ///TODO: create an dummy object and attach part to it
            droppingPart.Drop();
        }
    }
}
