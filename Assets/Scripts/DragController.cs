using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] Camera cam;
    public DragDestination currentDest;
    Ray ray;
    [SerializeField] float minDistance = 2f;
    [SerializeField] float maxDistance = 5f;
    public float floatSpeed = 0.5f;
    public bool onSurface = false;
    public bool onDestination = false;
    LayerMask surfaceMask => LayerMask.GetMask("Surface");
    LayerMask destinationMask => LayerMask.GetMask("Drag Destination");
    Vector3 newPos;
    Quaternion newRot;


    public void Drag(GameObject obj, Rigidbody rb)
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        ray = new Ray(cam.transform.position, cam.transform.forward);

        newPos = Vector3.zero;
        newRot = Quaternion.identity;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, destinationMask, QueryTriggerInteraction.Collide))
        {
            currentDest = hit.transform.GetComponent<DragDestination>();
            if (currentDest != null)
            {
                newPos = currentDest.snapPosition;
                newRot = currentDest.snapRot;

                onDestination = true;
            }
        }
        else
        {
            onDestination = false;
            FindNearestPoint(obj);
        }

        obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, newRot, Time.deltaTime * floatSpeed);

        obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * floatSpeed);
    }

    void FindNearestPoint(GameObject obj)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, LayerMask.GetMask("Default")))
        {
            if (hit.transform.gameObject.tag.Equals("Surface"))
            {
                newPos = hit.point + new Vector3(0, 0.15f, 0);
                newRot = obj.GetComponent<IInteractable>().ogRot;
                onSurface = true;
            }
            else
            {
                Vector3 pos = cam.transform.position + cam.transform.forward * (hit.distance - 0.2f);
                newPos = pos;

                Vector3 temp = hit.transform.gameObject.transform.position - obj.transform.position;
                newRot = Quaternion.LookRotation(temp);
                onSurface = false;
            }
        }
        else
        {
            newPos = cam.transform.position + cam.transform.forward * minDistance;
            Vector3 rot = cam.transform.position - obj.transform.position;
            newRot = Quaternion.LookRotation(rot);
            onSurface = false;
        }
    }

    public void StopDrag(Rigidbody rb)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        onDestination = false;
        onSurface = false;
        rb.useGravity = true;
    }

    public void StartPositionReset(IInteractable interactable)
    {
        StartCoroutine(SmoothPositionReset(interactable));
    }

    IEnumerator SmoothPositionReset(IInteractable interactable)
    {

        while (interactable.transform.position != interactable.lastPos && interactable.transform.rotation != interactable.lastRot)
        {
            interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactable.lastPos, Time.deltaTime * floatSpeed);
            interactable.transform.rotation = Quaternion.Lerp(interactable.transform.rotation, interactable.lastRot, Time.deltaTime * floatSpeed);
            yield return null;
        }

        StopDrag(interactable.rb);
    }
}
