using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : JobTask
{
    [HideInInspector] public DragDestination destination;
    [HideInInspector] public Vector3 velocity;

    public override void DragAction()
    {
        base.DragAction();
        active = true;
    }

    public override void StopDragAction()
    {
        active = false;
    }
}