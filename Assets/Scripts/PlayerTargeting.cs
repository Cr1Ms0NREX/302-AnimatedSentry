using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{

    public float visionDistance = 10;

    public TargetableObjects target { get; private set; }
    public bool playerWantsToAim { get; private set; }

    private List<TargetableObjects> validTargets = new List<TargetableObjects>();
    private float cooldownScan = 0;
    private float cooldownPickTarget = 0;

    void Update()
    {
        playerWantsToAim = Input.GetButton("Fire2");

        cooldownScan -= Time.deltaTime;
        cooldownPickTarget -= Time.deltaTime;

        if (playerWantsToAim)
        {
            if (cooldownScan <= 0) ScanForTargets();
            if (cooldownPickTarget <= 0) PickATarget();
        }
        else
        {
            target = null;
        }

        print(target);
    }

    void ScanForTargets()
    {
        cooldownScan = .5f;
        validTargets.Clear();
        TargetableObjects[] things = GameObject.FindObjectsOfType<TargetableObjects>();
        foreach(TargetableObjects thing in things)
        {
            Vector3 vToThing = thing.transform.position - transform.position;

            // Is close enough to see?
            if (vToThing.sqrMagnitude < visionDistance * visionDistance)
            {
                float alignment = Vector3.Dot(transform.forward, vToThing.normalized);
                // is within <180 degrees of forward
                if (alignment > .4f)
                {
                    validTargets.Add(thing);
                }
            }
        }
    }
    void PickATarget()
    {
        if (target) return;
        float closestDistanceSoFar = 0;
        foreach(TargetableObjects thing in validTargets)
        {
            Vector3 vToThing = thing.transform.position - transform.position;
            float dis = vToThing.sqrMagnitude;
            if (dis < closestDistanceSoFar || target == null)
            {
                closestDistanceSoFar = dis;
                target = thing;
            }
        }
    }

}
