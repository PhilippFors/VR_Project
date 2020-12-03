using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] Camera cam;
    public DragDestination currentDest;
    public Transform helper;
    Ray ray;
    public float maxDistance = 5f;
    public float floatSpeed = 6f;
    public bool onSurface = false;
    public bool onDestination = false;
    public float offsetXZ = 0f;
    public float offsetY = 0.15f;

    LayerMask destinationMask => LayerMask.GetMask("Drag Destination");
    public LayerMask defaultMask;
    Vector3 newPos;
    Quaternion newRot;
    RaycastHit hit;
    Vector3 oldPos;

    public void Drag(IInteractable obj, Rigidbody rb, bool throwable)
    {
        if (throwable)
        {
            rb.useGravity = false;
            if (Physics.Raycast(ray, out hit, maxDistance, defaultMask))
            {
                if (hit.transform.gameObject.tag.Equals("Surface"))
                {
                    onSurface = true;
                }
                else
                {
                    onSurface = false;
                }
            }
            oldPos = newPos;
            newPos = cam.transform.position + cam.transform.forward * Vector3.Distance(cam.transform.position, obj.lastPos);
            obj.GetComponent<Draggable>().velocity = newPos - oldPos;

            onSurface = false;
        }

        if (!throwable)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            ray = new Ray(cam.transform.position, cam.transform.forward);
            oldPos = newPos;

            newPos = Vector3.zero;
            newRot = Quaternion.identity;

            FindDestination(obj);

            obj.GetComponent<Draggable>().velocity = newPos - oldPos;
        }

        obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, newRot, throwable ? Time.deltaTime * (floatSpeed + 2) : Time.deltaTime * floatSpeed);
        obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, throwable ? Time.deltaTime * (floatSpeed + 2) : Time.deltaTime * floatSpeed);
    }

    void FindDestination(IInteractable obj)
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
                            Vector3 temp = cam.transform.position - hit.point;
                            Vector3 tempNormal = temp.normalized;
                            newPos = hit.point + new Vector3(tempNormal.x * offsetXZ, offsetY, tempNormal.z * offsetXZ);
                            newRot = obj.GetComponent<IInteractable>().ogRot;
                            onSurface = true;
                        }
                        else
                        {
                            newPos = helper.position;
                            newRot = helper.rotation;
                            onSurface = false;
                        }
                    }
                }
                else
                {
                    newPos = helper.position;
                    newRot = helper.rotation;
                    onSurface = false;
                }
            }
            else
            {
                currentDest = null;
                FindNearestPoint(obj);
            }
        }
        else if (Physics.Raycast(ray, out hit, maxDistance, destinationMask))
        {
            currentDest = hit.transform.GetComponent<DragDestination>();
            newPos = currentDest.snapPosition;
            newRot = currentDest.snapRot;
        }
        else
        {
            currentDest = null;
            FindNearestPoint(obj);
        }
    }

    void FindNearestPoint(IInteractable obj)
    {
        if (Physics.Raycast(ray, out hit, maxDistance, defaultMask))
        {
            if (hit.transform.gameObject.tag.Equals("Surface"))
            {
                Vector3 temp = cam.transform.position - hit.point;
                Vector3 tempNormal = temp.normalized;
                newPos = hit.point + new Vector3(tempNormal.x * offsetXZ, offsetY, tempNormal.z * offsetXZ);
                newRot = obj.GetComponent<IInteractable>().ogRot;
                onSurface = true;
            }
            else
            {
                newPos = helper.position;
                newRot = helper.rotation;
                onSurface = false;
            }
        }
        else
        {
            newPos = helper.position;
            newRot = helper.rotation;
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
        yield return InteractUtilities.instance.StartSmoothPositionChange(interactable, interactable.lastPos, interactable.lastRot, true, true);

        interactable.StopDragAction();
        StopDrag(interactable.rb);
    }
}
