using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    Ray ray;
    public LayerMask raycastMasks;
    [SerializeField] IInteractable interactionObj;
    [SerializeField] Camera cam;
    [SerializeField] DragController dragger;


    [SerializeField] float minholdtime = 0.5f;
    [SerializeField] float minForDragTime = 0.2f;

    [Header("Throw settings")]
    [SerializeField] float throwImpulse = 3f;
    Touch touch;
    float touchtime;

    public bool isHovering = false;
    public bool isHolding = false;
    public bool isDragging = false;

    void Update()
    {
        FindObject();

        // HoldSimulation();

        ThrowInteraction();

        CheckForHoldAction();

        DragInteraction();
    }

    //Used for Debugging in the Editor
    void HoldSimulation()
    {
        RaycastHit hit;
        ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out hit, dragger.maxDistance + 2, raycastMasks, QueryTriggerInteraction.Ignore) & !isHolding & !isDragging)
        {
            var inter = hit.transform.gameObject.GetComponent<IInteractable>();
            if (inter != null)
            {
                interactionObj = inter;
                touchtime += Time.deltaTime;
                if (touchtime >= minholdtime)
                {
                    if (interactionObj.draggable | interactionObj.throwable & !interactionObj.active)
                        isDragging = true;

                    if (interactionObj.holdable)
                        isHolding = true;
                }
            }
        }

        if (isHolding)
            interactionObj.HoldAction();

        if (isDragging && interactionObj.draggable)
            Dragging();

        if (isDragging && interactionObj.throwable)
            PickUp();

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
            {
                if (interactionObj.draggable)
                    StopDragging();
                if (interactionObj.throwable)
                    LetGo();
            }
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

    #region Throw Interaction

    void ThrowInteraction()
    {
        if (interactionObj != null && interactionObj.throwable)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    touchtime += Time.deltaTime;
                    if (touchtime >= minForDragTime)
                    {
                        PickUp();
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    touchtime = 0;
                    if (isDragging)
                    {
                        LetGo();
                    }
                }
            }
        }
    }

    void PickUp()
    {
        if (!interactionObj.GetComponent<Draggable>().active)
        {
            dragger.Drag(interactionObj, interactionObj.rb, true);
            isDragging = true;
        }
    }

    void LetGo()
    {
        interactionObj.rb.useGravity = true;

        Vector3 vel = interactionObj.GetComponent<Draggable>().velocity;
        Vector3 velNormal = vel.normalized;
        interactionObj.rb.AddForce(velNormal * throwImpulse * vel.magnitude, ForceMode.Impulse);
        interactionObj.rb.angularVelocity = new Vector3(Random.Range(10f, 200f) * vel.magnitude, Random.Range(10f, 200) * vel.magnitude, Random.Range(10f, 200) * vel.magnitude);
        // interactionObj.rb.velocity = vel;
        interactionObj.ThrowAction();
        isDragging = false;
    }

    #endregion

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
            dragger.Drag(interactionObj, interactionObj.rb, false);
            isDragging = true;
        }
    }

    void StopDragging()
    {
        var draggable = interactionObj.GetComponent<Draggable>();

        if (draggable.destination != null && draggable.destination.onDestination)
        {
            if (!dragger.currentDest.active && dragger.currentDest.pairObj.name.Equals(interactionObj.name))
            {
                interactionObj.transform.position = dragger.currentDest.snapPosition;
                interactionObj.transform.rotation = dragger.currentDest.snapRot;
                dragger.StopDrag(interactionObj.rb);
                interactionObj.DragAction();
                dragger.currentDest.WaitForCompletionStart();
                dragger.currentDest = null;
            }
            else
            {
                dragger.StartPositionReset(interactionObj);
            }
        }
        else if (dragger.onSurface)
        {
            dragger.StopDrag(interactionObj.rb);
        }
        else if (!dragger.onSurface && !dragger.onDestination)
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

        ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out hit, dragger.maxDistance + 2, raycastMasks, QueryTriggerInteraction.Ignore) & !isHolding & !isDragging)
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

                isHovering = false;
                interactionObj = null;
            }
        }
    }
}

