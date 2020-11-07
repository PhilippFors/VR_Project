using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector] public Vector3 ogPos;
    [HideInInspector] public Quaternion ogRot;
    Rigidbody rb => GetComponent<Rigidbody>();
    bool reset;
    public bool draggable;
    public Dragging dragger;
    public abstract void HoldAction();

    public abstract void StopHold();

    public virtual void DragAction()
    {
        if (draggable)
        {
            reset = false;
            dragger.Drag(gameObject, rb);
        }
    }

    public virtual void StopDrag()
    {
        if (draggable)
            if (!dragger.onSurface)
            {
                StartCoroutine(ResetDrag());
            }
            else
            {
                Ray ray = new Ray(rb.gameObject.transform.position, Vector3.down);
                if (Physics.SphereCast(ray, 0.4f, 1f, LayerMask.GetMask("Interactable")))
                {
                    StartCoroutine(ResetDrag());
                }
                else
                {
                    dragger.StopDrag(rb);
                }
            }
    }

    public abstract void PointerEnter();

    public abstract void PointerExit();

    public abstract void PointerClick();

    public IEnumerator ResetDrag()
    {
        bool reset = false;
        while (!reset)
        {
            transform.position = Vector3.Lerp(transform.position, ogPos, Time.deltaTime * dragger.floatSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, ogRot, Time.deltaTime * dragger.floatSpeed);
            if (transform.position == ogPos)
                reset = true;
            yield return null;
        }

        dragger.StopDrag(rb);
    }

    public void ResetPosition()
    {
        transform.position = ogPos;
        transform.rotation = ogRot;
    }
}
