using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductivityButton : JobTask
{
    // Start is called before the first frame update
    public Animation anim;

    public override void PointerClick()
    {
        base.PointerClick();
        anim.Play();
    }
}
