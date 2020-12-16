using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperTask : JobTask
{
    [SerializeField] string[] texts;
    int count;
    [SerializeField] TMPro.TextMeshProUGUI paperText;
    public override void INIT()
    {
        base.INIT();
        paperText.text = texts[0];
        count++;
    }
    public override void StopDragAction()
    {
        base.StopDragAction();
        if (count < texts.Length)
        {
            paperText.text = texts[count];
            count++;
        }

    }
}
