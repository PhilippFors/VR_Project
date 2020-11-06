using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    public Transform DragHelper;
    public Vector3 ogPos;
    public Camera cam;
    Ray ray;

    public float minDistance = 2f;
    public float maxDistance = 5f;
    public float floatSpeed = 0.5f;

    public bool onSurface = false;
    public void Drag(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
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
        DragHelper.position = newPos;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * floatSpeed);
    }

    public void StopDrag(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().useGravity = true;
    }




}
