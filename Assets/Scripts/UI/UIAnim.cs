using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnim : IInteractable
{
    [SerializeField] TaskUI ui;
    public Image TaskRingBG;
    public Image TaskRingFG;
    public Image middleBG;
    Vector3 ogTaskRingScale;
    Vector3 ogFGRingScale;
    Vector3 ogMiddlBGScale;
    public float ringEndScale;
    public float FGRingEndScale;
    public float middleEndPos;
    public float FGEndPos;

    [Header("BG Indicator Colors")]
    public Color fine;
    public Color uhOh;
    public Color danger;
    private void Start()
    {
        ogTaskRingScale = TaskRingBG.transform.localScale;
        ogFGRingScale = TaskRingFG.transform.localScale;
        ogMiddlBGScale = middleBG.transform.localScale;
    }
    public override void PointerEnter()
    {
        TaskRingBG.transform.DOScale(ogTaskRingScale + new Vector3(ringEndScale, ringEndScale, 0), 0.8f);

        middleBG.transform.DOScale(ogMiddlBGScale + new Vector3(ringEndScale, ringEndScale, 0), 0.8f);
        // middleBG.transform.DOLocalMoveZ(middleEndPos, 0.8f);

        TaskRingFG.transform.DOLocalMoveZ(FGEndPos, 0.8f);
        TaskRingFG.transform.DOScale(ogFGRingScale + new Vector3(FGRingEndScale, FGRingEndScale, 0), 0.8f);
    }

    public override void PointerExit()
    {
        TaskRingBG.transform.DOScale(ogTaskRingScale, 0.8f);

        middleBG.transform.DOScale(ogMiddlBGScale, 0.8f);
        // middleBG.transform.DOLocalMoveZ(0, 0.8f);

        TaskRingFG.transform.DOLocalMoveZ(0, 0.8f);
        TaskRingFG.transform.DOScale(ogFGRingScale, 0.8f);
    }

    private void Update()
    {
        if (ui.task.currentTaskCompletionValue >= 70f)
            middleBG.color = fine;
        else if (ui.task.currentTaskCompletionValue < 70f && ui.task.currentTaskCompletionValue >= 40f)
            middleBG.color = uhOh;
        else
            middleBG.color = danger;
    }
}
