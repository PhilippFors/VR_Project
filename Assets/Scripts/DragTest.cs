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
        ogPos = transform.position;
        ogRot = transform.rotation;
    }

    public override void PointerExit()
    {

    }



    public override void StopHold()
    {

    }
}
