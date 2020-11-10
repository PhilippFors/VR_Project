using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    Ray ray;
    Interactable iObject;
    Interactable oldObj;
    [SerializeField] Camera cam;
    float touchtime;
    public float minholdtime = 1f;
    Touch touch;

    bool isHovering = false;
    bool isHolding = false;

    void Update()
    {
        FindObject();

        HoldSimulation();

        CheckForHoldAction();
    }


    //Used for Debugging in the Editor
    void HoldSimulation()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.SphereCast(ray, 0.1f, out hit) & !isHolding)
        {
            if (hit.transform.gameObject.GetComponent<JobTask>() != null)
            {
                touchtime += Time.deltaTime;
                if (touchtime >= minholdtime)
                {
                    if (iObject != null)
                    {
                        isHolding = true;
                        iObject = hit.transform.GetComponent<JobTask>();
                    }
                }
            }
        }

        if (isHolding)
        {
            iObject.DragAction();
            iObject.HoldAction();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchtime = 0;
            if (isHolding)
                if (iObject != null)
                {
                    iObject.StopDrag();
                    iObject.StopHold();
                    isHolding = false;
                }
        }
    }

    //Works only on mobild build
    public void CheckForHoldAction()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                touchtime += Time.deltaTime;
                if (touchtime >= minholdtime)
                {
                    if (iObject != null)
                    {

                        iObject.DragAction();

                        iObject.HoldAction();

                        isHolding = true;

                    }

                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                touchtime = 0;
                if (isHolding)
                    if (iObject != null)
                    {
                        iObject.StopDrag();

                        iObject.StopHold();

                        isHolding = false;
                    }
            }
        }
    }

    //Sends a Raycast and looks for an Object with the 'Interactable' component on it to save it into a variable
    void FindObject()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        GameObject obj = null;
        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            if (hit.transform.gameObject.GetComponent<JobTask>() != null)
            {
                if (iObject == null)
                {
                    iObject = hit.transform.gameObject.GetComponent<JobTask>();
                    obj = hit.transform.gameObject;
                }
                if (iObject != null & !isHovering & !isHolding)
                {
                    iObject.PointerEnter();
                    isHovering = true;
                }
            }
            else
            {
                if (!isHolding)
                {
                    if (iObject != null)
                        iObject.PointerExit();
                    oldObj = iObject;
                    isHovering = false;
                    iObject = null;
                }
            }
        }
        else
        {
            if (!isHolding)
            {
                if (iObject != null)
                    iObject.PointerExit();
                oldObj = iObject;
                isHovering = false;
                iObject = null;
            }
        }

    }

}

