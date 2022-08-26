using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider sliderBar;

    public void SetHealthBar(float health)
    {
        sliderBar.value = health;
    }

    public void SetMaxHealthBar(float health)
    {
        sliderBar.maxValue = health;
        sliderBar.value = health;
    }
}
