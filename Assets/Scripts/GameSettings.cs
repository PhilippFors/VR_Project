using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public bool godMode = false;
    public float dragFloatspeed;
    public float tweenSpeed;
    public float maxRayDist = 5f;
    public static GameSettings instance;
    public TMPro.TextMeshProUGUI godmodeToggle;
    void Awake()
    {
        instance = this;
    }

    public void ToggleGodMode()
    {
        godMode = !godMode;
        if (!godMode)
            godmodeToggle.text = "Godmode off";
        else
            godmodeToggle.text = "Godmode on";
    }
}
