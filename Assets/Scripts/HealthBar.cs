using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider component not assigned.");
        }
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(int health) 
    {
        slider.value = Mathf.Clamp(health, 0, slider.maxValue);
    }
}
