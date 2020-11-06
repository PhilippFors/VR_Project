using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
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
    }

    public abstract void PointerEnter();

    public abstract void PointerExit();

    public abstract void PointerClick();

    IEnumerator ResetDrag()
    {
        while (transform.position != dragger.ogPos)
        {
            transform.position = Vector3.Lerp(transform.position, dragger.ogPos, Time.deltaTime * dragger.floatSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, dragger.ogRot, Time.deltaTime * dragger.floatSpeed);

            yield return null;
        }

        dragger.StopDrag(rb);
    }
}
