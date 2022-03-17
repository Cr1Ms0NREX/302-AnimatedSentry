using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform boneLegLeft;
    public Transform boneLegRight;
    public Transform boneHip;
    public Transform boneSpine;
    public float walkSpeed;
    //CharacterController pawn;
    NavMeshAgent agent;
    Transform navTarget;
    private Vector3 navTargetDir;
    [Range(-10, -1)]
    public float gravity = -1;
    private float velocityVertical = 0;
    private float cooldownJumpWindow = 0;
    /*public bool IsGrounded
    {
        get
        {
            return agent.isGrounded || cooldownJumpWindow > 0;
        }
    }*/
    // Start is called before the first frame update
    void Start()
    {
        //pawn = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5;
        
        PlayerTargeting player = FindObjectOfType<PlayerTargeting>();
        navTarget = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownJumpWindow > 0) cooldownJumpWindow -= Time.deltaTime;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        navTargetDir = transform.forward * v + transform.right * h;
        if (navTargetDir.sqrMagnitude > 1) navTargetDir.Normalize();

        if (navTarget) agent.destination = navTarget.transform.position;
        Vector3 moveAmount = navTargetDir * walkSpeed * velocityVertical;
        agent.Move(moveAmount * Time.deltaTime);
        if (agent)
        {
            cooldownJumpWindow = .5f;
            velocityVertical = 0;
            WalkAnimation();
        }
        else
        {
            AirAnimation();
        }
    }
    void WalkAnimation()
    {

        Vector3 navTargetDirLocal = transform.InverseTransformDirection(navTargetDir);
        Vector3 axis = Vector3.Cross(Vector3.up, navTargetDir);
        float alignment = Vector3.Dot(navTargetDirLocal, Vector3.forward);
        alignment = Mathf.Abs(alignment);

        float degrees = AnimMath.Lerp(10, 40, alignment);
        float speed = 10;
        float wave = Mathf.Sin(Time.time * speed) * degrees;

        boneLegLeft.localRotation = Quaternion.AngleAxis(wave, axis);
        boneLegRight.localRotation = Quaternion.AngleAxis(-wave, axis);

        if (boneHip)
        {
            float walkAmount = axis.magnitude;
            float offsetY = Mathf.Sin(Time.time * speed) * walkAmount * .05f;
            boneHip.localPosition = new Vector3(0, offsetY, 0);

        }

    }
    void AirAnimation()
    {

    }
}
