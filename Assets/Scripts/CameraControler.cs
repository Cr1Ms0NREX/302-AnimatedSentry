using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{

    public Transform target;

    public float mouseSenseX = 5;
    public float mouseSenseY = -5;
    public float mouseSenseScroll = -5;

    private Camera cam;

    private float pitch = 0;
    private float yaw = 0;
    private float dollyDis = 10;
    private float targetOffset;


    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }


    void Update()
    {
        // Movement
        if (target)
        {
            transform.position = AnimMath.Ease(transform.position, target.position + new Vector3(0, 0, 1), .01f);
        }
       
        // Rotation
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        yaw += mx * mouseSenseX;
        pitch += my * mouseSenseY;

        pitch = Mathf.Clamp(pitch, -20, 89);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        // Dolly Camera

        dollyDis += Input.mouseScrollDelta.y * mouseSenseScroll;
        dollyDis = Mathf.Clamp(dollyDis, 3, 20);
        cam.transform.localPosition = AnimMath.Ease(
            cam.transform.localPosition,
            new Vector3(0, 0, -dollyDis),
            .02f);

    }

    private void OnDrawGizmos()
    {
        if (!cam) cam = GetComponentInChildren<Camera>();
        if (!cam) return;

        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }


}
