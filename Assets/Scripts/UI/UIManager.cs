using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject interactableIndicator;
    public Camera cam;
    public Slider throwSlider;

    #region Interactable Indicator
    public void EnableIndicator(IInteractable interactionObj)
    {
        TMPro.TextMeshProUGUI t = interactableIndicator.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (interactionObj.clickable)
            t.text = "Click";
        else if (interactionObj.holdable)
            t.text = "Hold";
        else if (interactionObj.lookable)
        {
            t.text = "Look";
            StartCoroutine(WaitforIndicatorDisable());
        }
        else if (interactionObj.draggable)
            t.text = "Drag";
        else if (interactionObj.throwable)
            t.text = "Throw";

        if (Physics.CheckSphere(interactionObj.transform.position + new Vector3(0, 0.4f, 0), 0.02f))
        {
            interactableIndicator.transform.position = interactionObj.transform.position + new Vector3(0, 0.6f, 0);
        }
        else
        {
            interactableIndicator.transform.position = interactionObj.transform.position + new Vector3(0, 0.4f, 0);
        }

        interactableIndicator.transform.rotation = Quaternion.LookRotation(-(cam.transform.position - interactableIndicator.transform.position));
        interactableIndicator.SetActive(true);
    }

    public void DisableIndicator()
    {
        interactableIndicator.SetActive(false);
    }

    IEnumerator WaitforIndicatorDisable()
    {
        yield return new WaitForSeconds(2f);
        DisableIndicator();
    }
    #endregion
}
