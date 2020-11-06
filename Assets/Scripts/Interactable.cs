using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool draggable;
    public abstract void HoldAction();

    public abstract void StopHold();

    public abstract void DragAction();

    public abstract void PointerEnter();

    public abstract void PointerExit();

    public abstract void PointerClick();
}
