using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStateDisplay : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    public void SetHealthValue(float percent)
    {
        healthBar.fillAmount = percent;
    }
}
