using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnim : IInteractable
{
    public Image TaskRing;
    public Image TaskName;
    Vector3 ogTaskRingScale;
    Vector3 ogTaskNameScale;

    public Vector3 ringEndScale;
    public Vector3 nameEndScale;
    private void Start()
    {
        ogTaskRingScale = TaskRing.transform.localScale;
        ogTaskNameScale = TaskName.transform.localScale;

    }
    public override void PointerEnter()
    {
        TaskRing.transform.DOScale(ringEndScale, 0.8f);
        TaskName.transform.DOScale(nameEndScale, 0.8f);
    }

    public override void PointerExit()
    {
        TaskRing.transform.DOScale(ogTaskRingScale, 0.8f);
        TaskName.transform.DOScale(ogTaskNameScale, 0.8f);
    }
}
