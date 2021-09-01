using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMP_Text healthText;

    public void SetmaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.SetText($"{health} / {health}");

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health, int maxHealth)
    {
        slider.value = health;
        healthText.SetText($"{health} / {maxHealth}");

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
