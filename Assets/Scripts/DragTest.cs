using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : Interactable
{
    public override void HoldAction()
    {

    }

    public override void PointerClick()
    {

    }

    public override void PointerEnter()
    {
        dragger.ogPos = transform.position;
        dragger.ogRot = transform.rotation;
    }

    public override void PointerExit()
    {

    }



    public override void StopHold()
    {

    }
}
