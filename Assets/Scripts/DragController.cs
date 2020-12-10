using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] Camera cam;
    public DragDestination currentDest;
    public Transform helper;
    Ray ray;

    public bool onSurface = false;
    public bool onDestination = false;
    [SerializeField] float offsetXZ = 0f;
    [SerializeField] float offsetY = 0.15f;

    LayerMask destinationMask => LayerMask.GetMask("Drag Destination");
    public LayerMask defaultMask;

    RaycastHit hit;

    Vector3 newPos;
    Vector3 oldPos;
    Quaternion newRot;

    [SerializeField] float minDistance = 3f;

    bool checkForCols = true;

    #region Cols
    BoxCollider boxCol = null;
    MeshCollider meshCol = null;
    CapsuleCollider capCol = null;
    SphereCollider sphCol = null;
    #endregion

    public void Drag(IInteractable obj, Rigidbody rb, bool throwable)
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        oldPos = newPos;

        if (throwable)
        {
            newPos = cam.transform.position + cam.transform.forward * minDistance;
            newRot = Quaternion.identity;

            onSurface = false;
        }

        if (!throwable)
        {
            ray = new Ray(cam.transform.position, cam.transform.forward);

            newPos = Vector3.zero;
            newRot = Quaternion.identity;

            FindDestination(obj);
        }

        obj.GetComponent<Draggable>().velocity = newPos - oldPos;
        obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, newRot, throwable ? Time.deltaTime * (GameSettings.instance.dragFloatspeed + 2) : Time.deltaTime * GameSettings.instance.dragFloatspeed);
        obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, throwable ? Time.deltaTime * (GameSettings.instance.dragFloatspeed + 2) : Time.deltaTime * GameSettings.instance.dragFloatspeed);
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
                if (checkForCols)
                {
                    DisableCol(obj);
                    checkForCols = false;
                }

                newPos = currentDest.snapPosition;
                newRot = currentDest.snapRot;

                if (Physics.Raycast(ray, out hit, GameSettings.instance.maxRayDist, defaultMask))
                {
                    if (Vector3.Distance(hit.point, currentDest.snapPosition) > 0.5f)
                    {
                        if (hit.transform.gameObject.tag.Equals("Surface"))
                        {
                            ShutOffDest(obj);
                            EnableCol();
                            MoveOnSurface(obj);
                        }
                        else
                        {
                            ShutOffDest(obj);
                            EnableCol();
                            MoveToHelper();
                        }
                    }
                }
                else
                {
                    ShutOffDest(obj);
                    EnableCol();
                    MoveToHelper();
                }
            }
            else
            {
                FindNearestPoint(obj);
            }
        }
        else if (Physics.Raycast(ray, out hit, GameSettings.instance.maxRayDist, destinationMask))
        {
            currentDest = hit.transform.GetComponent<DragDestination>();
            newPos = currentDest.snapPosition;
            newRot = currentDest.snapRot;
        }
        else
        {
            FindNearestPoint(obj);
        }
    }

    void FindNearestPoint(IInteractable obj)
    {
        ShutOffDest(obj);
        EnableCol();
        if (Physics.Raycast(ray, out hit, GameSettings.instance.maxRayDist, defaultMask))
        {
            if (hit.transform.gameObject.tag.Equals("Surface"))
            {
                MoveOnSurface(obj);
            }
            else
            {
                MoveToHelper();
            }
        }
        else
        {
            MoveToHelper();
        }
    }

    void MoveOnSurface(IInteractable obj)
    {
        Vector3 temp = cam.transform.position - hit.point;
        Vector3 tempNormal = temp.normalized;
        newPos = hit.point + new Vector3(tempNormal.x * offsetXZ, offsetY, tempNormal.z * offsetXZ);
        newRot = obj.GetComponent<IInteractable>().ogRot;
        onSurface = true;
    }

    void MoveToHelper()
    {
        newPos = helper.position;
        newRot = helper.rotation;
        onSurface = false;
    }

    public void StopDrag(Rigidbody rb)
    {
        EnableCol();
        checkForCols = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        onDestination = false;
        onSurface = false;
        rb.useGravity = true;

        boxCol = null;
        meshCol = null;
        capCol = null;
        sphCol = null;
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

    void DisableCol(IInteractable i)
    {
        if (InteractUtilities.instance.FindCol<BoxCollider>(i, out boxCol))
            boxCol.enabled = false;

        else if (InteractUtilities.instance.FindCol<CapsuleCollider>(i, out capCol))
            capCol.enabled = false;

        else if (InteractUtilities.instance.FindCol<MeshCollider>(i, out meshCol))
            meshCol.enabled = false;

        else if (InteractUtilities.instance.FindCol<SphereCollider>(i, out sphCol))
            sphCol.enabled = false;
    }

    void ShutOffDest(IInteractable obj)
    {
        obj.destination = null;
        if (currentDest != null)
        {
            currentDest.onDestination = false;
            currentDest = null;
        }
    }

    void EnableCol()
    {
        if (boxCol != null)
            boxCol.enabled = true;

        else if (meshCol != null)
            meshCol.enabled = true;

        else if (capCol != null)
            capCol.enabled = true;

        else if (sphCol != null)
            sphCol.enabled = true;

    }
}
