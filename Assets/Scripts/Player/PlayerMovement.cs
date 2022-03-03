using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Player BodyPart Movement
    public Transform boneLegLeft;
    public Transform boneLegRight;
    public Transform boneHip;
    public Transform boneSpine;
    public float walkSpeed;
    [Range(-10, -1)]
    public float gravity = -1;

    // Camera
    public Camera cam;

    // Player Controls
    CharacterController pawn;
    PlayerTargeting targetingScript;
    private Vector3 inputDir;
    private float velocityVertical = 0;
    private bool isDead = false;
    private float cooldownJumpWindow = 0;

    public bool IsGrounded
    {
        get
        {
            return pawn.isGrounded || cooldownJumpWindow > 0;
        }
    }

    public void Start()
    {
        pawn = GetComponent<CharacterController>();
        targetingScript = GetComponent<PlayerTargeting>();
    }


    public void Update()
    {
        if (cooldownJumpWindow > 0) cooldownJumpWindow -= Time.deltaTime;
        //float v = Input.GetAxisRaw("Vertical");
        //float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        bool playerWantsToMove = (v != 0 || h != 0);

        bool playerIsAiming = (targetingScript && targetingScript.playerWantsToAim && targetingScript.target);

        if (playerIsAiming) {
            Vector3 toTarget = targetingScript.target.transform.position - transform.position;
            toTarget.Normalize();
            Quaternion worldRot = Quaternion.LookRotation(toTarget);
            Vector3 euler = worldRot.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            worldRot.eulerAngles = euler;

            transform.rotation = AnimMath.Ease(transform.rotation, worldRot, .01f);
        }else if (cam && playerWantsToMove)
        {
            float playerYaw = transform.eulerAngles.y;
            float camYaw = cam.transform.eulerAngles.y;

            while (camYaw > playerYaw + 180) camYaw -= 360;
            while (camYaw < playerYaw - 180) camYaw += 360;

            //print($"camYaw: {camYaw} playerYaw: {playerYaw}");

            Quaternion playerRotation = Quaternion.Euler(0, playerYaw, 0); 
            Quaternion targetRotation = Quaternion.Euler(0, camYaw, 0);
            transform.rotation = AnimMath.Ease(playerRotation, targetRotation, .01f);
        }

        inputDir = transform.forward * v + transform.right * h;
        if (inputDir.sqrMagnitude > 1) inputDir.Normalize();

        bool wantsToJump = Input.GetButtonDown("Jump");
        if(IsGrounded)
        {
            velocityVertical = 0;
            if (wantsToJump)
            {
                cooldownJumpWindow = 0;
                velocityVertical = 8;
            }
        }
        else
        {
            velocityVertical += gravity * Time.deltaTime;
        }
        velocityVertical += gravity * Time.deltaTime;

        Vector3 moveAmount = inputDir * walkSpeed + Vector3.up * velocityVertical;
        pawn.Move(moveAmount * Time.deltaTime);
        if (pawn.isGrounded) {
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

        Vector3 inputDirLocal = transform.InverseTransformDirection(inputDir);
        Vector3 axis = Vector3.Cross(Vector3.up, inputDir);
        float alignment = Vector3.Dot(inputDirLocal, Vector3.forward);
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
    void IsDead()
    {
        if (isDead == true)
        {

        }
    }
    void AirAnimation()
    {

    }
}
