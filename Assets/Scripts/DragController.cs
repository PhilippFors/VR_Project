using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] Camera cam;
    public DragDestination currentDest;
    Ray ray;
    [SerializeField] float minDistance = 2f;
    public float maxDistance = 5f;
    public float floatSpeed = 9f;
    public bool onSurface = false;
    public bool onDestination = false;
    LayerMask surfaceMask => LayerMask.GetMask("Surface");
    LayerMask destinationMask => LayerMask.GetMask("Drag Destination");
    [SerializeField] LayerMask defaultMask;
    Vector3 newPos;
    Quaternion newRot;
    RaycastHit hit;

    public void Drag(GameObject obj, Rigidbody rb)
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        ray = new Ray(cam.transform.position, cam.transform.forward);

        newPos = Vector3.zero;
        newRot = Quaternion.identity;

        FindDestination(obj);

        obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, newRot, Time.deltaTime * floatSpeed);
        obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * floatSpeed);
    }

    void FindDestination(GameObject obj)
    {
        Vector3 pos = Vector3.zero;
        var draggable = obj.GetComponent<Draggable>();

        if (draggable.destination != null)
        {
            currentDest = draggable.destination;

            if (draggable.destination.onDestination)
            {
                newPos = currentDest.snapPosition;
                newRot = currentDest.snapRot;

                if (Physics.Raycast(ray, out hit, maxDistance, defaultMask))
                {
                    if (Vector3.Distance(hit.point, currentDest.snapPosition) > 0.5f)
                    {
                        if (hit.transform.gameObject.tag.Equals("Surface"))
                        {
                            newPos = hit.point + new Vector3(0, 0.15f, 0);
                            newRot = obj.GetComponent<IInteractable>().ogRot;
                            onSurface = true;
                        }
                        else
                        {
                            pos = cam.transform.position + cam.transform.forward * (hit.distance - 0.2f);
                            newPos = pos;

                            Vector3 temp = hit.transform.gameObject.transform.position - obj.transform.position;
                            newRot = Quaternion.LookRotation(temp);
                            onSurface = false;
                        }
                    }
                }
            }
            else
            {
                currentDest = null;
                FindNearestPoint(obj);
            }
        }
        else
        {
            FindNearestPoint(obj);
        }
    }

    void FindNearestPoint(GameObject obj)
    {
        if (Physics.Raycast(ray, out hit, maxDistance, defaultMask))
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
        yield return InteractUtilities.instance.StartSmoothPositionChange(interactable, interactable.lastPos, interactable.lastRot);

        StopDrag(interactable.rb);
    }
}
