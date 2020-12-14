using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StressreliefTask : IInteractable
{
    [SerializeField] float stressReductionValue;

    private void Awake()
    {
        ogPos = transform.position;
        ogRot = transform.rotation;

        throwCounter = maxThrows;
        interactable = true;
    }

    public override void HoldAction()
    {
        if (!holdable)
            return;
        else
            RelievStressOverTime();
    }

    public override void PointerClick()
    {
        if (!clickable)
            return;
        else
            RelieveStressOnce();
    }

    protected void RelieveStressOnce()
    {
        StressManager.instance.RelieveStress(stressReductionValue);
    }

    protected void RelievStressOverTime()
    {
        StressManager.instance.RelieveStress(stressReductionValue * Time.deltaTime);
    }
}
