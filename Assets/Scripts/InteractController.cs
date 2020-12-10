using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{

    [SerializeField] DragController dragger;
    [SerializeField] UIManager uiManager;

    [SerializeField] IInteractable interactionObj;
    [SerializeField] Camera cam;

    [SerializeField] float minholdtime = 0.5f;

    [Header("Throw settings")]
    [SerializeField] float sweepSpeed = 7f;
    [SerializeField] FloatVariable maxForce;
    [SerializeField] FloatVariable currentForce;
    Ray ray;
    public LayerMask raycastMasks;
    Touch touch;
    float touchtime;

    bool isHovering = false;
    bool isHolding = false;
    bool isDragging = false;

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
        if (Physics.Raycast(ray, out hit, GameSettings.instance.maxRayDist + 2, raycastMasks, QueryTriggerInteraction.Ignore) & !isHolding & !isDragging)
        {
            var inter = hit.transform.gameObject.GetComponent<IInteractable>();
            if (inter != null)
            {
                if (inter.interactable)
                {
                    interactionObj = inter;
                    touchtime += Time.deltaTime;
                    if (touchtime >= minholdtime)
                    {
                        if (interactionObj.draggable | interactionObj.throwable & interactionObj.interactable)
                            isDragging = true;

                        if (interactionObj.holdable)
                            isHolding = true;
                    }
                }
            }
        }

        if (isHolding)
        {
            uiManager.DisableIndicator();
            interactionObj.HoldAction();
        }

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
        if (interactionObj != null && interactionObj.holdable & interactionObj.interactable)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    touchtime += Time.deltaTime;
                    if (touchtime >= minholdtime)
                    {
                        uiManager.DisableIndicator();
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
        if (interactionObj != null && interactionObj.throwable & interactionObj.interactable)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    touchtime += Time.deltaTime;
                    if (touchtime >= minholdtime)
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

    bool coActive = false;
    void PickUp()
    {
        uiManager.DisableIndicator();

        dragger.Drag(interactionObj, interactionObj.rb, true);
        isDragging = true;

        if (!coActive)
        {
            StartCoroutine(ThrowForce());
            coActive = true;
        }
    }

    void LetGo()
    {
        interactionObj.rb.useGravity = true;

        if (currentForce.Value >= 1f)
        {
            interactionObj.rb.AddForce(cam.transform.forward * currentForce.Value, ForceMode.Impulse);
            interactionObj.rb.angularVelocity = new Vector3(Random.Range(10f, 200f), Random.Range(10f, 200), Random.Range(10f, 200));
            interactionObj.interactable = false;
        }
        else
        {
            dragger.StartPositionReset(interactionObj);
        }
        isDragging = false;
        coActive = false;
    }

    IEnumerator ThrowForce()
    {
        currentForce.Value = 0f;
        uiManager.throwSlider.value = 0f;
        uiManager.throwSlider.gameObject.SetActive(true);
        bool tick = true;
        while (isDragging)
        {
            if (currentForce.Value <= 0)
                tick = true;
            if (currentForce.Value >= maxForce.Value)
                tick = false;

            if (tick)
                currentForce.Value += Time.deltaTime * sweepSpeed;
            else
                currentForce.Value -= Time.deltaTime * sweepSpeed;

            yield return null;
        }

        uiManager.throwSlider.gameObject.SetActive(false);
    }
    #endregion

    #region Drag Interaction
    void DragInteraction()
    {
        if (interactionObj != null && interactionObj.draggable & interactionObj.interactable)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    touchtime += Time.deltaTime;
                    if (touchtime >= minholdtime)
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
        uiManager.DisableIndicator();
        dragger.Drag(interactionObj, interactionObj.rb, false);
        isDragging = true;
    }


    void StopDragging()
    {
        if (interactionObj.destination != null && interactionObj.destination.onDestination)
        {
            if (!interactionObj.destination.active && interactionObj.destination.pairObj.name.Equals(interactionObj.name))
            {
                interactionObj.transform.position = interactionObj.destination.snapPosition;
                interactionObj.transform.rotation = interactionObj.destination.snapRot;
                dragger.StopDrag(interactionObj.rb);
                interactionObj.DragAction();
                interactionObj.destination.WaitForCompletionStart();
                interactionObj.destination = null;
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
        if (Physics.Raycast(ray, out hit, GameSettings.instance.maxRayDist + 2f, raycastMasks, QueryTriggerInteraction.Ignore) & !isHolding & !isDragging)
        {

            var inter = hit.transform.gameObject.GetComponent<IInteractable>();
            if (inter != null && inter.interactable)
            {
                if (interactionObj == null)
                {
                    interactionObj = inter;
                    if (!isHovering & !isHolding & !isDragging)
                    {
                        uiManager.EnableIndicator(interactionObj);
                        interactionObj.PointerEnter();
                        isHovering = true;
                    }
                }
            }
            else
            {
                EscapeObject();
            }
        }
        else
        {
            EscapeObject();
        }
    }

    void EscapeObject()
    {
        if (!isHolding & !isDragging)
        {
            if (interactionObj != null)
            {
                uiManager.DisableIndicator();
                interactionObj.PointerExit();
            }

            isHovering = false;
            interactionObj = null;
        }
    }
}

