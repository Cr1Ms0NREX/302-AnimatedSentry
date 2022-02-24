using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{

    public PlayerTargeting player;

    private Vector3 targetOffset;

    public float mouseSenseX = 5;
    public float mouseSenseY = -5;
    public float mouseSenseScroll = -5;

    private Camera cam;

    private float pitch = 0;
    private float yaw = 0;
    private float dollyDis = 10;


    // how many seconds to shake...
    private float shakeTimer = 0;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        if (player == null)
        {
            PlayerTargeting script = FindObjectOfType<PlayerTargeting>();
            if (script != null) player = script;
        }
    }


    void Update()
    {

        bool isAiming = (player && player.target && player.playerWantsToAim);
        // 1. Ease Postistion
        if (player)
        {
            transform.position = AnimMath.Ease(transform.position, player.transform.position + targetOffset, .01f);
        }

            float playerYaw = AnimMath.AngleWrapDegrees(yaw, player.transform.eulerAngles.y);
            //while (yaw > playerYaw + 180) playerYaw += 360;
            //while (yaw < playerYaw - 180) playerYaw -= 360;
        // 2. Set Rotation
        if (isAiming)
        {
            
            Quaternion tempTarget = Quaternion.Euler(0, playerYaw, 0);
            transform.rotation = AnimMath.Ease(transform.rotation,  tempTarget, .001f);
            // Point at Target...
        }
        else
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            yaw += mx * mouseSenseX;
            pitch += my * mouseSenseY;
            
            
            
            pitch = Mathf.Clamp(pitch, -20, 89);
            transform.rotation = AnimMath.Ease(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .001f);
        }
        
        // 3. Dolly Camera
        dollyDis += Input.mouseScrollDelta.y * mouseSenseScroll;
        dollyDis = Mathf.Clamp(dollyDis, 3, 10);
        float tempZ = isAiming ? 2 : dollyDis;

        cam.transform.localPosition = AnimMath.Ease(
            cam.transform.localPosition,
            new Vector3(0, 0, -tempZ),
            .02f);

        // 4. rotate the camera

        if (isAiming)
        {
            Vector3 vToAimTarget = player.target.transform.position - cam.transform.position;
            Quaternion worldRot = Quaternion.LookRotation(vToAimTarget);
            Quaternion localRot = worldRot;

            if (cam.transform.parent)
            {
                localRot = Quaternion.Inverse(cam.transform.parent.rotation) * worldRot;
            }
            Vector3 euler = localRot.eulerAngles;
            euler.x = 0;
            localRot.eulerAngles = euler;
            cam.transform.localRotation = AnimMath.Ease(cam.transform.localRotation, localRot, .001f);
        }
        else
        {
            cam.transform.localRotation = AnimMath.Ease(cam.transform.localRotation, Quaternion.identity, .001f);
        }
        UpdateShake();
    }
    void UpdateShake()
    {
        if (shakeTimer < 0) return;
        shakeTimer -= Time.deltaTime;
        float p = shakeTimer / 1;
        p = p * p;
        p = AnimMath.Lerp(1, .98f, p);
        Quaternion randomRot = AnimMath.Lerp(Random.rotation, Quaternion.identity, p);
        cam.transform.localRotation *= randomRot;
    }
    public void Shake(float time)
    {
        //if (time > shakeTimer) 
        shakeTimer += time;
    }
    private void OnDrawGizmos()
    {
        if (!cam) cam = GetComponentInChildren<Camera>();
        if (!cam) return;

        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }


}
