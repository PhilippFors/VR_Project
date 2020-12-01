using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] Camera cam;
    public DragDestination currentDest;
    public Transform helper;
    Ray ray;
    [SerializeField] float minDistance = 2f;
    public float maxDistance = 5f;
    public float floatSpeed = 6f;
    public bool onSurface = false;
    public bool onDestination = false;
    public float offsetSingle = 0f;
    public Vector3 offset = new Vector3(0f, 0.15f, 0f);
    LayerMask surfaceMask => LayerMask.GetMask("Surface");
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

            // Vector3 rot = cam.transform.position - obj.transform.position;
            // obj.transform.rotation = Quaternion.LookRotation(rot);
            // rb.MovePosition(newPos);
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

        obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, newRot, throwable ? Time.deltaTime * (floatSpeed + 5) : Time.deltaTime * floatSpeed);
        obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, throwable ? Time.deltaTime * (floatSpeed + 5) : Time.deltaTime * floatSpeed);
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
                            Vector3 targetPos = new Vector3(tempNormal.x * offsetSingle, hit.point.y, tempNormal.z * offsetSingle);
                            newPos = targetPos + offset;
                            newRot = obj.GetComponent<IInteractable>().ogRot;
                            onSurface = true;
                        }
                        else
                        {
                            // pos = cam.transform.position + cam.transform.forward * (hit.distance - 0.1f);
                            newPos = helper.position;
                            // Vector3 temp = hit.transform.gameObject.transform.position - obj.transform.position;
                            newRot = helper.rotation;
                            onSurface = false;
                        }
                    }
                }
                else
                {
                    // pos = cam.transform.position + cam.transform.forward * (hit.distance - 0.1f);
                    newPos = helper.position;

                    // Vector3 temp = hit.transform.gameObject.transform.position - obj.transform.position;
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
                RaycastHit check;
                // if (Physics.SphereCast(obj.transform.position, 1f, Vector3.up, out check, 1f, defaultMask))
                // {
                //     GameObject inTheW = check.transform.gameObject;
                //     if (Vector3.Distance(inTheW.transform.position, hit.point) < 1f)
                //     {
                //         Vector3 temp = hit.point - inTheW.transform.position;
                //         newPos = inTheW.transform.position + temp.normalized * 0.8f + offset;
                //         newRot = Quaternion.LookRotation(temp);
                //     }
                // }
                // else
                // {
                Vector3 temp = cam.transform.position - hit.point;
                Vector3 tempNormal = temp.normalized;
                Vector3 targetPos = hit.point + tempNormal * offsetSingle;
                newPos = new Vector3(targetPos.x, hit.point.y, targetPos.z) + offset;
                newRot = obj.GetComponent<IInteractable>().ogRot;
                onSurface = true;
                // }
            }
            else
            {
                // Vector3 pos = cam.transform.position + cam.transform.forward * (hit.distance - 0.1f);
                // newPos = pos;

                // Vector3 temp = obj.transform.position - hit.transform.gameObject.transform.position;
                // newRot = Quaternion.LookRotation(temp);
                // onSurface = false;
                // newPos = cam.transform.position + cam.transform.forward * minDistance;
                newPos = helper.position;
                // Vector3 rot = cam.transform.position - obj.transform.position;
                newRot = helper.rotation;
                onSurface = false;
            }
        }
        else
        {
            // newPos = cam.transform.position + cam.transform.forward * minDistance;
            newPos = helper.position;
            // Vector3 rot = cam.transform.position - obj.transform.position;
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

        if (interactable.rb != null)
            interactable.rb.useGravity = false;
            
        yield return InteractUtilities.instance.StartSmoothPositionChange(interactable, interactable.lastPos, interactable.lastRot);

        interactable.StopDragAction();
        StopDrag(interactable.rb);
    }
}
