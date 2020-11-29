using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : JobTask
{
    public bool active = false;
    public DragDestination destination;

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
