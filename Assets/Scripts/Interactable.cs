using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector] public bool clickable, holdable, lookable, draggable;
    [HideInInspector] public float clickStressValue, holdStressValue, lookStressValue, dragStressValue;
    [HideInInspector] public float clickCompletionValue, holdCompletionValue, lookCompletionValue, dragCompletionValue;

    public float currentTaskCompletionValue;
    public float maxTaskCompletionValue;
    public float completionReductionValue;
    [HideInInspector] public Vector3 lastPos;
    [HideInInspector] public Vector3 ogPos => gameObject.transform.position;
    [HideInInspector] public Quaternion lastRot;
    [HideInInspector] public Quaternion ogRot => gameObject.transform.rotation;
    Rigidbody rb => GetComponent<Rigidbody>();
    bool reset;
    bool lookedAt;
    public Dragging dragger => FindObjectOfType<Dragging>();

    Coroutine coroutine;

    void Update()
    {
        if (lookedAt)
            AddCompletionOverTime(lookCompletionValue);
    }

    public virtual void HoldAction()
    {
        if (!holdable)
            return;
        else
        {
            HoldStress();
            AddCompletionOverTime(holdCompletionValue);
            StopCountdown();
        }
    }

    public virtual void StopHold()
    {
        StartCountdown();
    }

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
                StartCoroutine(SmoothPositionReset());
            }
            else
            {
                Ray ray = new Ray(rb.gameObject.transform.position, Vector3.down);
                if (Physics.SphereCast(ray, 0.4f, 1f, LayerMask.GetMask("Interactable")))
                {
                    StartCoroutine(SmoothPositionReset());
                }
                else
                {
                    dragger.StopDrag(rb);
                }
            }
    }

    public virtual void PointerEnter()
    {
        if (!lookable)
            return;
        else
        {
            LookStress();
            lookedAt = true;
            StopCountdown();
        }
    }

    public virtual void PointerExit()
    {
        lookedAt = false;
        StartCountdown();
    }

    public virtual void PointerClick()
    {
        if (!clickable)
            return;
        else
        {
            ClickStress();
            AddCompletionOnce(clickCompletionValue);
            RestartCountdown();
        }
    }

    #region stress Adders

    void ClickStress()
    {
        StressManager.instance.AddStress(clickStressValue);
    }

    void HoldStress()
    {
        StressManager.instance.AddStress(holdStressValue * Time.deltaTime);
    }

    void LookStress()
    {
        StressManager.instance.AddStress(lookStressValue * Time.deltaTime);
    }

    void DragStress()
    {
        StressManager.instance.AddStress(dragStressValue);
    }
    #endregion
    
    public IEnumerator SmoothPositionReset()
    {
        bool reset = false;
        while (!reset)
        {
            transform.position = Vector3.Lerp(transform.position, lastPos, Time.deltaTime * dragger.floatSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, lastRot, Time.deltaTime * dragger.floatSpeed);
            if (transform.position == lastPos)
                reset = true;
            yield return null;
        }

        dragger.StopDrag(rb);
    }

    public void ResetLastPosition()
    {
        transform.position = lastPos;
        transform.rotation = lastRot;
    }
    public void ResetOgPosition()
    {
        transform.position = ogPos;
        transform.rotation = ogRot;
    }

    void AddCompletionOnce(float value)
    {
        if (currentTaskCompletionValue + value > maxTaskCompletionValue)
            currentTaskCompletionValue = maxTaskCompletionValue;
        else
            currentTaskCompletionValue += value;
    }

    void AddCompletionOverTime(float value)
    {
        if (currentTaskCompletionValue + value * Time.deltaTime > maxTaskCompletionValue)
            currentTaskCompletionValue = maxTaskCompletionValue;
        else
            currentTaskCompletionValue += value * Time.deltaTime;
    }
    void StopCountdown()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
    void StartCountdown()
    {
        coroutine = StartCoroutine(TaskCompletionCountdown());
    }
    void RestartCountdown()
    {
        StopCountdown();

        StartCountdown();
    }
    IEnumerator TaskCompletionCountdown()
    {
        yield return new WaitForSeconds(2f);
        while (currentTaskCompletionValue >= 0)
            currentTaskCompletionValue -= completionReductionValue * Time.deltaTime;
    }
}
