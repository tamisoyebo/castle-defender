using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slide;

    public void SetHealth (int health)
    {
        slide.value = health;
    }

}
