using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressreliefTask : MonoBehaviour, Interactable
{
    [HideInInspector] public bool clickable, holdable, lookable, draggable;
    [HideInInspector] public Vector3 ogPos => gameObject.transform.position;
    [HideInInspector] public Quaternion ogRot => gameObject.transform.rotation;
    public float stressReductionValue;

    public void DragAction()
    {
        throw new System.NotImplementedException();
    }

    public void HoldAction()
    {
        if (!holdable)
            return;
        else
            StressManager.instance.RelieveStress(stressReductionValue * Time.deltaTime);
    }

    public void PointerClick()
    {
        throw new System.NotImplementedException();
    }

    public void PointerEnter()
    {
        throw new System.NotImplementedException();
    }

    public void PointerExit()
    {
        throw new System.NotImplementedException();
    }

    public void StopDrag()
    {
        throw new System.NotImplementedException();
    }

    public void StopHold()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
