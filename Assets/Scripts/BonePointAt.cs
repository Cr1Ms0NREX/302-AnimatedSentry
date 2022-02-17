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

    public bool lockAxisX = false;
    public bool lockAxisY = false;
    public bool lockAxisZ = false;

    public Axis aimOrientation;

    private Quaternion startRotation;
    private Quaternion goalRotation;
    private PlayerTargeting playerTargeting;

    void Start()
    {
        playerTargeting = GetComponentInParent<PlayerTargeting>();
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardsTarget();
    }

    private void TurnTowardsTarget()
    {
        if (playerTargeting && playerTargeting.target && playerTargeting.playerWantsToAim)
        {
            Vector3 vToTarget = playerTargeting.target.transform.position - transform.position;
            vToTarget.Normalize();
            Quaternion worldRot = Quaternion.LookRotation(vToTarget, Vector3.up);
            Quaternion prevRot = transform.rotation;
            Vector3 eulerBefore = transform.localEulerAngles;
            transform.rotation = worldRot;
            Vector3 eulerAfter = transform.localEulerAngles;
            transform.rotation = prevRot;


            /*Quaternion localRot = worldRot;
            if (transform.parent)
            {
                localRot = Quaternion.Inverse(transform.parent.rotation) * worldRot;
            }*/
            //Vector3 euler = localRot.eulerAngles;

            if (lockAxisX) eulerAfter.y = eulerBefore.x;
            if (lockAxisX) eulerAfter.x = eulerBefore.y;
            if (lockAxisX) eulerAfter.z = eulerBefore.z;
            goalRotation = Quaternion.Euler(eulerAfter);
            //localRot.eulerAngles = euler;
            //goalRotation = localRot;
        } else
        {
            goalRotation = startRotation;
        }

        transform.localRotation = AnimMath.Ease(transform.localRotation, goalRotation, .001f);
    }
}
