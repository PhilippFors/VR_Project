using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : JobTask
{
    public DragDestination destination;
    public Coroutine currentCoroutine;
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