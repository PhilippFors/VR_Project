using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressManager : MonoBehaviour
{
    public FloatVariable currentStress;
    public FloatVariable maxStress;

    public static StressManager instance;

    public TMPro.TextMeshProUGUI ui;
    public float penaltyMultiplier = 1.0f;
    public float penaltyAmount;
    public float penaltyTime;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdatePenaltyUI();
        MyEventSystem.instance.penaltyTrigger += AddPenalty;
        currentStress.Value = 0;
    }

    public void AddStress(float value)
    {
        currentStress.Value += value * penaltyMultiplier;

        if (currentStress.Value >= maxStress.Value)
        {
            if (!GameSettings.instance.godMode)
            {
                GameManager.instance.Death();
            }
        }
        else if (currentStress.Value < 0)
            currentStress.Value = 0;
    }

    public void RelieveStress(float value)
    {
        if (currentStress.Value - value <= 0)
            currentStress.Value = 0;
        else
            currentStress.Value -= value;
    }

    void AddPenalty()
    {
        StartCoroutine(PenaltyTimer());
    }

    IEnumerator PenaltyTimer()
    {
        penaltyMultiplier += penaltyAmount;
        UpdatePenaltyUI();

        yield return new WaitForSeconds(penaltyTime);

        penaltyMultiplier -= penaltyAmount;
        UpdatePenaltyUI();

        if (penaltyMultiplier <= 1f)
            penaltyMultiplier = 1.0f;
    }

    void UpdatePenaltyUI()
    {
        ui.text = "x" + penaltyMultiplier.ToString("F");
    }

    public void ResetStress()
    {
        currentStress.Value = 0;
    }

    public void SetAllTasksFull()
    {
        JobTask[] tasks = GameObject.FindObjectsOfType<JobTask>();
        foreach (JobTask task in tasks)
        {
            task.currentTaskCompletionValue = 100;
            task.StartTaskReduction();
        }
    }
}
