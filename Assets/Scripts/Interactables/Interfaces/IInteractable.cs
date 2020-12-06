using UnityEngine;
using System.Collections;

[System.Serializable]
public struct InteractableDesc
{
    public string name;

    public string description;
}

[RequireComponent(typeof(Rigidbody))]
public abstract class IInteractable : MonoBehaviour
{
    public InteractableDesc information;

    [HideInInspector] public bool clickable, holdable, lookable, draggable, throwable;

    //When true, can be interacted with
    public bool interactable;
    [HideInInspector] public Vector3 lastPos;
    [HideInInspector] public Quaternion lastRot;
    [HideInInspector] public Vector3 ogPos;
    [HideInInspector] public Quaternion ogRot;


    //For Draggable interactions
    [HideInInspector] public DragDestination destination;
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public Rigidbody rb => GetComponent<Rigidbody>();

    private void Start()
    {
        interactable = true;
    }
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
    public virtual void ThrowAction()
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
