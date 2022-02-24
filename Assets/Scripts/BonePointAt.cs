using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Axis
{
    Forward,
    Backward,
    Right,
    Left,
    Up,
    Down
}

public class BonePointAt : MonoBehaviour
{
    public Transform target;
    public float weightAxisX = 0;
    public float weightAxisY = 0;
    public float weightAxisZ = 0;
    public bool lockAxisX = false;
    public bool lockAxisY = false;
    public bool lockAxisZ = false;
    public Axis aimOrientation;

    private Quaternion startRotation;
    private Quaternion goalRotation;
    private PlayerTargeting playerTargeting;

    void Start()
    {
        //playerTargeting = GetComponentInParent<PlayerTargeting>();
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardsTarget();
    }

    private void TurnTowardsTarget()
    {
        //if (playerTargeting && playerTargeting.target && playerTargeting.playerWantsToAim)
        if(target != null)
        {
            Vector3 vToTarget = target.position - transform.position;
            vToTarget.Normalize();
            Quaternion worldRot = Quaternion.LookRotation(vToTarget, Vector3.up);
            Quaternion localRot = worldRot;
            // Convert to local space:
            if (transform.parent)
            {
                localRot = Quaternion.Inverse(transform.parent.rotation) * worldRot;
            }
            Vector3 euler = localRot.eulerAngles;
            if (lockAxisX) euler.x = startRotation.eulerAngles.x;
            if (lockAxisY) euler.y = startRotation.eulerAngles.y;
            if (lockAxisZ) euler.z = startRotation.eulerAngles.z;
            localRot.eulerAngles = euler;
            goalRotation = localRot;

            /*Quaternion worldRot = Quaternion.LookRotation(vToTarget, Vector3.up);
            Quaternion prevRot = transform.rotation;
            Vector3 eulerBefore = transform.localEulerAngles;
            transform.rotation = worldRot;
            Vector3 eulerAfter = transform.localEulerAngles;
            transform.rotation = prevRot;*/
            //Quaternion localRot = worldRot;
            /*if (transform.parent)
            {
                localRot = Quaternion.Inverse(transform.parent.rotation) * worldRot;
            }
            //Vector3 euler = localRot.eulerAngles;

            if (lockAxisX) eulerAfter.x = eulerBefore.x;
            if (lockAxisY) eulerAfter.y = eulerBefore.y;
            if (lockAxisZ) eulerAfter.z = eulerBefore.z;
            goalRotation = Quaternion.Euler(eulerAfter);*/
            //localRot.eulerAngles = euler;
            //goalRotation = localRot;
        } else
        {
            goalRotation = startRotation;
        }

        transform.localRotation = AnimMath.Ease(transform.localRotation, goalRotation, .001f);
    }
}
