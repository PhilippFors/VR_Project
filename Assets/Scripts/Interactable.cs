using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    void HoldAction();
    void StopHold();
    void DragAction();
    void StopDrag();
    void PointerEnter();
    void PointerExit();
    void PointerClick();
}
