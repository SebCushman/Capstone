using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    public Slider slider;
    //public Gradient gradient;
    public Image fill;
    public Color fillColor;

    public void SetCooldown(float cooldown)
    {
        slider.maxValue = cooldown;
        fillColor = fill.color;
        //fill.color = gradient.Evaluate(1f);
    }
    public void SetCurrentTimer(float cooldown)
    {
        slider.value = cooldown;

        if(slider.value == slider.maxValue)
        {
            fillColor.a = 0;
            fill.color = fillColor;
        }
        else
        {
            fillColor.a = .4f;
            fill.color = fillColor;
        }

        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
