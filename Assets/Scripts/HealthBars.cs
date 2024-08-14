using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    [SerializeField] protected Image healthBar;

    public void HealthBarUpdate(float newPercentage)
    {
        healthBar.fillAmount = newPercentage;
    }
}
