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



    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        if (player == null)
        {
            PlayerTargeting script = FindObjectOfType<PlayerTargeting>();

        }
    }


    void Update()
    {

        bool isAiming = (player && player.target && player.playerWantsToAim);
        // Movement
        if (player)
        {
            transform.position = AnimMath.Ease(transform.position, player.transform.position + new Vector3(0, 0, 1), .01f);
        }

            float playerYaw = AnimMath.AngleWrapDegrees(yaw, player.transform.eulerAngles.y);
            while (yaw > playerYaw + 180) playerYaw += 360;
            while (yaw < playerYaw - 180) playerYaw -= 360;
        // Rotation
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
        
        // Dolly Camera
        dollyDis += Input.mouseScrollDelta.y * mouseSenseScroll;
        dollyDis = Mathf.Clamp(dollyDis, 3, 20);
        float tempZ = isAiming ? 2 : dollyDis;

        cam.transform.localPosition = AnimMath.Ease(
            cam.transform.localPosition,
            new Vector3(0, 0, -dollyDis),
            .02f);

        // 4. rotate the camera

        if (isAiming)
        {
            Vector3 vToAimTarget = player.target.transform.position - cam.transform.position;
            Vector3 euler = Quaternion.LookRotation(vToAimTarget).eulerAngles;

            euler.y = AnimMath.AngleWrapDegrees(playerYaw, euler.y);

            while (playerYaw > euler.y + 180) euler.y += 360;
            while (playerYaw < euler.y - 180) euler.y -= 360;
            Quaternion temp = Quaternion.Euler(euler.x, euler.y, 0);
            cam.transform.rotation = AnimMath.Ease(cam.transform.rotation, temp, .001f);
        }
        else
        {
            cam.transform.localRotation = AnimMath.Ease(cam.transform.localRotation, Quaternion.identity, .001f);
        }

    }

    private void OnDrawGizmos()
    {
        if (!cam) cam = GetComponentInChildren<Camera>();
        if (!cam) return;

        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }


}
