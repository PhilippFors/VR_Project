using UnityEngine;
using System.Collections;

[System.Serializable]
public struct InteractableDesc
{
    public string name;

    public string description;
}

public abstract class IInteractable : MonoBehaviour
{
    public InteractableDesc information;

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
