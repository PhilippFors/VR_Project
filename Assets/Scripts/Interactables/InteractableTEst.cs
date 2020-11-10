using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTEst : JobTask
{
    public override void HoldAction()
    {
        base.HoldAction();
        Debug.Log("I am " + gameObject.name);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void StopHold()
    {
        base.StopHold();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void PointerEnter()
    {
        base.PointerEnter();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void PointerExit()
    {
        base.PointerExit();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public override void PointerClick()
    {
        base.PointerClick();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
    }
}
