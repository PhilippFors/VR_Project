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
        ogTaskRingScale = TaskRingBG.transform.localScale;
        ogFGRingScale = TaskRingFG.transform.localScale;
        ogMiddlBGScale = middleBG.transform.localScale;
        
    }
    public override void PointerEnter()
    {
        TaskRingBG.transform.DOScale(ogTaskRingScale + new Vector3(ringEndScale, ringEndScale, 0), animSpeed);

        // middleBG.transform.DOScale(ogMiddlBGScale + new Vector3(ringEndScale, ringEndScale, 0), 0.8f);
        // middleBG.transform.DOLocalMoveZ(middleEndPos, 0.8f);

        TaskRingFG.transform.DOLocalMoveZ(FGEndPos, animSpeed);
        TaskRingFG.transform.DOScale(ogFGRingScale + new Vector3(FGRingEndScale, FGRingEndScale, 0), animSpeed);
    }

    public override void PointerExit()
    {
        TaskRingBG.transform.DOScale(ogTaskRingScale, animSpeed);

        // middleBG.transform.DOScale(ogMiddlBGScale, 0.8f);
        // middleBG.transform.DOLocalMoveZ(0, 0.8f);

        TaskRingFG.transform.DOLocalMoveZ(0, animSpeed);
        TaskRingFG.transform.DOScale(ogFGRingScale, animSpeed);
    }



}
