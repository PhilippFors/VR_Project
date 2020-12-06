using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public float dragFloatspeed;
    public float tweenSpeed;
    public float maxRayDist = 5f;
    public static GameSettings instance;
    void Awake()
    {
        instance = this;
    }
}
