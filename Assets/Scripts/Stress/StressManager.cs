using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressManager : MonoBehaviour
{
    public FloatVariable currentStress;
    public FloatVariable maxStress;

    public static StressManager instance;

    void Awake()
    {
        instance = this;
    }

    public void AddStress(float value)
    {
        if (currentStress.Value + value >= maxStress.Value)
            Debug.Log("You died");
        else
            currentStress.Value += value;
    }

    public void RelieveStress(float value)
    {
        if (currentStress.Value - value <= 0)
            currentStress.Value = 0;
        else
            currentStress.Value -= value;
    }
}
