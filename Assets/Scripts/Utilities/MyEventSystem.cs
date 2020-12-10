using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventSystem : MonoBehaviour
{
    public static MyEventSystem instance;

    public event System.Action penaltyTrigger;
    private void Awake()
    {
        instance = this;
    }

    public void PenaltyTrigger()
    {
        penaltyTrigger?.Invoke();
    }
}
