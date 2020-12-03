using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JobTask : IInteractable
{   
    [HideInInspector] public float clickStressValue, holdStressValue, lookStressValue, dragStressValue, throwStressValue;
    [HideInInspector] public float clickCompletionValue, holdCompletionValue, lookCompletionValue, dragCompletionValue, throwCompletionValue;

    public float currentTaskCompletionValue;
    public float maxTaskCompletionValue = 100f;
    
    [SerializeField] float completionReductionValue;
    [SerializeField] float waitForReduction = 2f;

    bool reset;
    bool lookedAt;
    Coroutine coroutine;

    private void Start()
    {
        ogPos = gameObject.transform.position;
        ogRot = gameObject.transform.rotation;
        currentTaskCompletionValue = Random.Range(30f, 90f);
        StartTaskReduction();
    }

    void Update()
    {
        if (lookedAt)
            AddCompletionOverTime(lookCompletionValue);
    }

    public override void HoldAction()
    {
        if (!holdable)
            return;
        else
        {
            AddStressOverTime(holdStressValue);
            AddCompletionOverTime(holdCompletionValue);
            StopTaskReduction();
        }
    }

    public override void StopHold()
    {
        StartTaskReduction();
    }

    public override void DragAction()
    {
        if (!draggable)
            return;
        else
        {
            AddStressOnce(dragStressValue);
            AddCompletionOnce(dragCompletionValue);
            RestartTaskReduction();
        }
    }

    public override void StopDragAction()
    {

    }

    public override void PointerEnter()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;

        if (!lookable)
            return;
        else
        {
            AddStressOverTime(lookStressValue);
            lookedAt = true;
            StopTaskReduction();
        }
    }

    public override void PointerExit()
    {
        if (!lookable)
            return;
        else
        {
            lookedAt = false;
            StartTaskReduction();
        }
    }

    public override void PointerClick()
    {
        if (!clickable)
            return;
        else
        {
            AddStressOnce(clickStressValue);
            AddCompletionOnce(clickCompletionValue);
            RestartTaskReduction();
        }
    }

    public override void ThrowAction()
    {
        if (!throwable)
            return;
        else
        {
            AddStressOnce(throwStressValue);
            AddCompletionOnce(throwCompletionValue);
        }
    }
    #region stress Adders

    void AddStressOnce(float stress)
    {
        StressManager.instance.AddStress(stress);
    }

    void AddStressOverTime(float stress)
    {
        StressManager.instance.AddStress(stress * Time.deltaTime);
    }

    #endregion

    public void ResetLastPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = lastPos;
        transform.rotation = lastRot;
    }

    public void ResetOgPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = ogPos;
        transform.rotation = ogRot;
    }

    protected void AddCompletionOnce(float value)
    {
        if (currentTaskCompletionValue + value > maxTaskCompletionValue)
            currentTaskCompletionValue = maxTaskCompletionValue;
        else
            currentTaskCompletionValue += value;
    }

    protected void AddCompletionOverTime(float value)
    {
        if (currentTaskCompletionValue + value * Time.deltaTime > maxTaskCompletionValue)
            currentTaskCompletionValue = maxTaskCompletionValue;
        else
            currentTaskCompletionValue += value * Time.deltaTime;
    }

    protected void StopTaskReduction()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    protected void StartTaskReduction()
    {
        coroutine = StartCoroutine(TaskCompletionCountdown());
    }

    protected void RestartTaskReduction()
    {
        StopTaskReduction();

        StartTaskReduction();
    }

    protected IEnumerator TaskCompletionCountdown()
    {
        yield return new WaitForSeconds(waitForReduction);
        while (currentTaskCompletionValue >= 0)
        {
            currentTaskCompletionValue -= completionReductionValue * Time.deltaTime;
            yield return null;
        }
    }
}
