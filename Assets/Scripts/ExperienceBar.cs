using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider slider;
    //public Gradient gradient;
    public Image fill;
    public TMP_Text levelText;

    public void SetmaxXP(int xp)
    {
        slider.maxValue = xp;
        slider.value = FindObjectOfType<Player>().currentXP;

        //fill.color = gradient.Evaluate(1f);
    }
    public void SetXP(int xp)
    {
        slider.value = xp;

        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
