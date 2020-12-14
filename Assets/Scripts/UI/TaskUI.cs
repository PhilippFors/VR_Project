using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TaskUI : MonoBehaviour
{
    public JobTask task;
    public Image image;
    public Image BG;

    public TextMeshProUGUI counter;
    public GameObject counterParent;

    [Header("FG Indicator Colors")]
    public Color fine;
    public Color uhOh;
    public Color danger;


    [Header("BG Indicator Colors")]
    public Color BGfine;
    public Color BGuhOh;
    public Color BGdanger;

    private void Awake()
    {
        UpdateCounter();
        task.updateThrowCounter += UpdateCounter;
    }

    private void OnDisable()
    {
        task.updateThrowCounter -= UpdateCounter;
    }
    void Update()
    {
        if (task.throwable)
        {
            counterParent.SetActive(true);
            UpdateCounter();
        }
        else
        {
            counterParent.SetActive(false);
        }

        if (image != null && BG != null)
        {
            image.fillAmount = task.currentTaskCompletionValue / task.maxTaskCompletionValue;

            if (task.currentTaskCompletionValue >= 65f)
            {
                image.color = fine;
                BG.color = BGfine;
            }
            else if (task.currentTaskCompletionValue < 65f && task.currentTaskCompletionValue >= 30f)
            {
                image.color = uhOh;
                BG.color = BGuhOh;
            }
            else
            {
                image.color = danger;
                BG.color = BGdanger;
            }
        }
    }

    void UpdateCounter()
    {
        counter.text = task.throwCounter.ToString();
    }
}
