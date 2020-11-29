using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    Ray ray;
    IInteractable interactionObj;
    IInteractable oldObj;
    [SerializeField] Camera cam;
    [SerializeField] DragController dragger;

    float touchtime;
    [SerializeField] float minholdtime = 0.5f;
    [SerializeField] float minForDragTime = 0.2f;

    Touch touch;

    bool isHovering = false;
    bool isHolding = false;
    bool isDragging = false;

    void Update()
    {
        FindObject();

        HoldSimulation();

        // CheckForHoldAction();

        // DragInteraction();
    }

    //Used for Debugging in the Editor
    void HoldSimulation()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out hit) & !isHolding & !isDragging)
        {
            if (hit.transform.gameObject.GetComponent<JobTask>() != null)
            {
                touchtime += Time.deltaTime;
                if (touchtime >= minholdtime)
                {
                    interactionObj = hit.transform.GetComponent<JobTask>();

                    if (interactionObj.draggable)
                        isDragging = true;
                    if (interactionObj.holdable)
                        isHolding = true;
                }
            }
        }

        if (isHolding)
            interactionObj.HoldAction();

        if (isDragging)
            Dragging();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchtime = 0;
            if (isHolding)
                if (interactionObj != null)
                {
                    if (interactionObj.holdable)
                        interactionObj.StopHold();

                    isHolding = false;
                }

            if (isDragging)
                if (interactionObj.draggable)
                    StopDragging();
        }
    }

    //Works only on mobile build
    void CheckForHoldAction()
    {
        if (interactionObj != null && interactionObj.holdable)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    touchtime += Time.deltaTime;
                    if (touchtime >= minholdtime)
                    {
                        interactionObj.HoldAction();

                        isHolding = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                touchtime = 0;
                if (isHolding)
                {
                    interactionObj.StopHold();

                    isHolding = false;
                }
            }
        }
    }

    #region Drag Interaction
    void DragInteraction()
    {
        if (interactionObj != null && interactionObj.draggable)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    touchtime += Time.deltaTime;
                    if (touchtime >= minForDragTime)
                    {
                        Dragging();
                    }

                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    touchtime = 0;
                    if (isDragging)
                    {
                        StopDragging();
                    }
                }
            }
        }
    }

    void Dragging()
    {
        if (!interactionObj.GetComponent<Draggable>().active)
        {
            dragger.Drag(interactionObj.gameObject, interactionObj.rb);
            isDragging = true;
        }
    }

    void StopDragging()
    {
        if (dragger.onDestination)
        {
            if (!dragger.currentDest.active && dragger.currentDest.pairObj.name.Equals(interactionObj.name))
            {
                dragger.StopDrag(interactionObj.rb);
                interactionObj.transform.position = dragger.currentDest.snapPosition;
                interactionObj.transform.rotation = dragger.currentDest.snapRot;
                interactionObj.DragAction();
                dragger.currentDest.WaitForCompletionStart();
                dragger.currentDest = null;
            }
        }
        else if (dragger.onSurface)
        {
            dragger.StopDrag(interactionObj.rb);
        }
        else if (!dragger.onSurface & !dragger.onDestination)
        {
            dragger.StartPositionReset(interactionObj);
        }

        isDragging = false;
    }

    #endregion

    //Sends a Raycast and looks for an Object with the 'Interactable' component on it to save it into a variable
    void FindObject()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out hit) & !isHolding & !isDragging)
        {
            var inter = hit.transform.gameObject.GetComponent<IInteractable>();
            if (inter != null)
            {
                if (interactionObj == null)
                {
                    interactionObj = inter;
                    if (!isHovering & !isHolding & !isDragging)
                    {
                        interactionObj.PointerEnter();
                        isHovering = true;
                    }
                }
            }
            else
            {
                if (!isHolding & !isDragging)
                {
                    if (interactionObj != null)
                        interactionObj.PointerExit();

                    oldObj = interactionObj;
                    isHovering = false;
                    interactionObj = null;
                }
            }
        }
        else
        {
            if (!isHolding & !isDragging)
            {
                if (interactionObj != null)
                    interactionObj.PointerExit();

                oldObj = interactionObj;
                isHovering = false;
                interactionObj = null;
            }
        }
    }
}

