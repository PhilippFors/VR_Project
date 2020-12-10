using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JobTask : IInteractable
{
    [HideInInspector] public float clickStressValue, holdStressValue, lookStressValue, dragStressValue, throwStressValue;
    [HideInInspector] public float clickCompletionValue, holdCompletionValue, lookCompletionValue, dragCompletionValue, throwCompletionValue;

    [Header("Task Settings")]
    public float currentTaskCompletionValue;
    public float maxTaskCompletionValue = 100f;

    [SerializeField] float completionReductionValue;
    [SerializeField] float waitForReduction = 2f;

    bool reset;
    bool lookedAt;
    Coroutine coroutine;

    public event System.Action updateThrowCounter;

    private void Start()
    {
        ogPos = gameObject.transform.position;
        ogRot = gameObject.transform.rotation;
        currentTaskCompletionValue = Random.Range(35f, 90f);
        StartTaskReduction();
    }

    void Update()
    {
        if (lookedAt)
        {
            AddCompletionOverTime(lookCompletionValue);
            AddStressOverTime(lookStressValue);
        }
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
            interactable = false;
            AddStressOnce(dragStressValue);
            AddCompletionOnce(dragCompletionValue);
            RestartTaskReduction();
        }
    }

    public override void StopDragAction()
    {
        interactable = true;
    }

    public override void PointerEnter()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;

        if (!lookable)
            return;
        else
        {
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

            if (throwCounter == maxThrows)
                StartCoroutine(ThrowReset());
            else
            {
                throwCounter--;
            }

            if (throwCounter < 0)
            {
                throwCounter = 0;
                MyEventSystem.instance.PenaltyTrigger();
            }

            updateThrowCounter?.Invoke();
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

    IEnumerator ThrowReset()
    {
        throwCounter--;
        while (throwCounter < maxThrows)
        {
            yield return new WaitForSeconds(waitCounterReset);
            throwCounter++;
            updateThrowCounter?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Surface") && throwable)
            interactable = true;
    }
}
