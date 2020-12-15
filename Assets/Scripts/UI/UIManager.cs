using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Camera cam;

    [Header("Throw Slider")]
    public Slider throwSlider;

    [Header("Interactable Indicator")]
    [SerializeField] GameObject interactableIndicator;

    [Header("Clock")]
    public TMPro.TextMeshProUGUI minuteText;
    public TMPro.TextMeshProUGUI secondText;

    private void Start()
    {
        MyEventSystem.instance.updateSecond += UpdateClockUISecond;
        MyEventSystem.instance.updateMinute += UpdateClockUIMinute;
    }

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

    void UpdateClockUISecond(int second)
    {
        if (second < 10)
            secondText.text = "0" + second.ToString();
        else
            secondText.text = second.ToString();
    }
    void UpdateClockUIMinute(int minute)
    {
        if (minute < 10)
            minuteText.text = "0" + minute.ToString();
        else
            minuteText.text = minute.ToString();
    }

    public void FlashClock()
    {

    }
}
