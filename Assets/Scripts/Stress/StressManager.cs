using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressManager : MonoBehaviour
{
    public float currentStress;

    public float maxStress;

    public static StressManager instance;

    void Awake()
    {
        instance = this;
    }

    public void AddStress(float value)
    {
        if (currentStress + value >= maxStress)
            Debug.Log("You died");
        else
            currentStress += value;
    }

    public void RelieveStress(float value)
    {
        if (currentStress - value <= 0)
            currentStress = 0;
        else
            currentStress -= value;
    }
}
