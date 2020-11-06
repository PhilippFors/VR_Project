using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTEst : Interactable
{
    public override void HoldAction()
    {
        Debug.Log("I am " + gameObject.name);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void StopHold()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void PointerEnter()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public override void PointerExit()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public override void PointerClick()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
    }


}
