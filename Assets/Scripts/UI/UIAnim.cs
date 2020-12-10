using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnim : IInteractable
{

    public float animSpeed = 0.5f;
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

    private void Start()
    {
        if (TaskRingBG != null)
            ogTaskRingScale = TaskRingBG.transform.localScale;
        if (TaskRingFG != null)
            ogFGRingScale = TaskRingFG.transform.localScale;
        if (middleBG != null)
            ogMiddlBGScale = middleBG.transform.localScale;
    }
    public override void PointerEnter()
    {
        if (TaskRingBG != null)
            TaskRingBG.transform.DOScale(ogTaskRingScale + new Vector3(ringEndScale, ringEndScale, 0), animSpeed);

        if (middleBG != null)
        {
            middleBG.transform.DOScale(ogMiddlBGScale + new Vector3(ringEndScale, ringEndScale, 0), animSpeed);
            middleBG.transform.DOLocalMoveZ(middleEndPos, animSpeed);
        }
        if (TaskRingFG != null)
        {
            TaskRingFG.transform.DOLocalMoveZ(FGEndPos, animSpeed);
            TaskRingFG.transform.DOScale(ogFGRingScale + new Vector3(FGRingEndScale, FGRingEndScale, 0), animSpeed);
        }
    }

    public override void PointerExit()
    {
        if (TaskRingBG != null)
            TaskRingBG.transform.DOScale(ogTaskRingScale, animSpeed);

        if (middleBG != null)
        {
            middleBG.transform.DOScale(ogMiddlBGScale, animSpeed);
            middleBG.transform.DOLocalMoveZ(0, animSpeed);
        }
        if (TaskRingFG != null)
        {
            TaskRingFG.transform.DOLocalMoveZ(0, animSpeed);
            TaskRingFG.transform.DOScale(ogFGRingScale, animSpeed);
        }
    }
}
