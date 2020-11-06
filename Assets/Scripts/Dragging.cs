using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    [HideInInspector] public Vector3 ogPos;
    [HideInInspector] public Quaternion ogRot;
    public Camera cam;
    Ray ray;

    public float minDistance = 2f;
    public float maxDistance = 5f;
    public float floatSpeed = 0.5f;

    [HideInInspector] public bool onSurface = false;

    public void Drag(GameObject obj, Rigidbody rb)
    {
        rb.useGravity = false;

        Vector3 newPos = Vector3.zero;

        RaycastHit hit;
        ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out hit, maxDistance, LayerMask.GetMask("Surface")))
        {
            newPos = hit.point;
            onSurface = true;
        }
        else
        {
            newPos = cam.transform.position + cam.transform.forward * minDistance;
            onSurface = false;
        }
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * floatSpeed);
    }

    public void StopDrag(Rigidbody rb)
    {
        rb.useGravity = true;
    }




}
