using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderUI : MonoBehaviour
{
    public FloatVariable currentValue;
    public FloatVariable maxValue;
    Slider slider => GetComponent<Slider>();

    private void Update()
    {
        SetSlider();
    }

    void SetSlider()
    {
        slider.value = currentValue.Value / maxValue.Value;
    }
}
