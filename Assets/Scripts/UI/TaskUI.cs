using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskUI : MonoBehaviour
{
    public JobTask task;
    public Image image;
    public TMPro.TextMeshProUGUI UiText;

    private void Start()
    {
        UiText.text = task.taskName;
    }
    void Update()
    {
        image.fillAmount = task.currentTaskCompletionValue / task.maxTaskCompletionValue;
    }
}
