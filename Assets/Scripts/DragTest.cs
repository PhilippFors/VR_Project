using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : Interactable
{
    bool reset = false;
    public Dragging dragger;
    public override void DragAction()
    {
        dragger.Drag(gameObject);

    }

    public override void HoldAction()
    {
        reset = false;
        if (draggable)
            DragAction();
    }

    public override void PointerClick()
    {

    }

    public override void PointerEnter()
    {
        dragger.ogPos = transform.position;
    }

    public override void PointerExit()
    {

    }

    public override void StopHold()
    {
        if (!dragger.onSurface)
            reset = true;
    }

    void Update()
    {
        if (reset)
            transform.position = Vector3.Lerp(transform.position, dragger.ogPos, Time.deltaTime * dragger.floatSpeed);

        if (transform.position == dragger.ogPos & reset)
        {
            reset = false;
            dragger.StopDrag(gameObject);
        }

    }
}
