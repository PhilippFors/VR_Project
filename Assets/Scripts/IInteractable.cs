using UnityEngine;
using System.Collections;


public abstract class IInteractable : MonoBehaviour
{
    [HideInInspector] public bool clickable, holdable, lookable, draggable, throwable;
    public bool active = false;
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

    public virtual void ThrowAction() { }
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
