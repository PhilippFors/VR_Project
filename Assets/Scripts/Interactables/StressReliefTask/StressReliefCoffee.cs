using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressReliefCoffee : StressreliefTask
{
    public Transform helper_close;

    public override void HoldAction()
    {
        transform.position = Vector3.Lerp(transform.position, helper_close.position, Time.deltaTime * 10f);
        transform.rotation = Quaternion.Lerp(transform.rotation, helper_close.rotation, Time.deltaTime + 10f);
        base.HoldAction();
    }

    public override void StopHold()
    {
        gameObject.transform.SetParent(null);
        InteractUtilities.instance.StartSmoothPositionChange(this, ogPos, ogRot);
    }

    // transform.parent = helper_close;
}
