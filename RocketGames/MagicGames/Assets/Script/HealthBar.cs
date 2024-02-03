using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healtSlider;
    public Gradient gradient;


    public Image fill;

    public void SetHealth(int health)
    {
        healtSlider.value = health;

        fill.color = gradient.Evaluate(healtSlider.normalizedValue);
    }

    public void SetMaxHealth(int health)
    {
        healtSlider.maxValue = health;
        healtSlider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
}
