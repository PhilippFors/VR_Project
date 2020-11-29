using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public abstract class IInteractable : MonoBehaviour
{
    [HideInInspector] public bool clickable, holdable, lookable, draggable;

    [HideInInspector] public Vector3 lastPos;
    [HideInInspector] public Quaternion lastRot;
    [HideInInspector] public Vector3 ogPos;
    [HideInInspector] public Quaternion ogRot;

    public Rigidbody rb => GetComponent<Rigidbody>();

    public virtual void HoldAction()
    {

    }
    public virtual void StopHold()
    {

    }
    public virtual void DragAction()
    {

    }
    public virtual void StopDragAction()
    {

    }
    public virtual void PointerEnter()
    {

    }
    public virtual void PointerExit()
    {

    }
    public virtual void PointerClick()
    {

    }

}
