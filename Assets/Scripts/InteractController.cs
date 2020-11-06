using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    Ray ray;
    public Transform DragHelper;
    Interactable iObject;
    Interactable oldObj;
    [SerializeField] Camera cam;
    public float touchtime;
    float minholdtime = 1f;
    Touch touch;

    bool isHovering = false;
    bool isHolding = false;
    bool canDrag = false;
    void Update()
    {
        FindObject();

        HoldSim();

        CheckForHoldAction();
    }

    void HoldSim()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Interactable>() != null)
            {
                touchtime += Time.deltaTime;
                if (touchtime >= minholdtime)
                {
                    if (iObject != null & !isHolding)
                    {
                        isHolding = true;
                        iObject = hit.transform.GetComponent<Interactable>();
                    }
                }
            }
        }
        if (isHolding)
            iObject.HoldAction();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchtime = 0;
            if (isHolding)
                if (iObject != null)
                {
                    isHolding = false;
                    iObject.StopHold();
                }
        }
    }


    void CheckForClick()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                //if (iObject != null)
                //iObject.PointerClick();
            }
        }
    }

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
                        isHolding = true;
                        iObject.HoldAction();
                    }

                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                touchtime = 0;
                if (isHolding)
                    if (iObject != null)
                    {
                        isHolding = false;
                        iObject.StopHold();
                    }
            }
        }
    }

    void FindObject()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        GameObject obj = null;
        if (Physics.SphereCast(ray, 0.2f, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Interactable>() != null)
            {
                if (iObject == null)
                {
                    iObject = hit.transform.gameObject.GetComponent<Interactable>();
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
                    DragHelper.parent = null;
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
                DragHelper.parent = null;
            }
        }



        if (obj != null & !canDrag)
        {
            DragHelper.position = obj.transform.position;
            DragHelper.parent = cam.transform;
            DragHelper.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}

