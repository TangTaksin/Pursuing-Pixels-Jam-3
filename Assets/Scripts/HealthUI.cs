using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public GameObject iconContainer;
    public TextMeshProUGUI healthLabel;
    public Image[] healthIcons;
    public Sprite OnSprite, offSprite;

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        var displayAmount = maxHealth - currentHealth;

        if (displayAmount < 0)
        {
            healthLabel.text = "Try Again ?";
            iconContainer.SetActive(false);

            return;
        }
         else if (displayAmount == 0)
        {
            healthLabel.text = "One Last Chance";
            iconContainer.SetActive(false);

            return;
        }
        else
        {
            healthLabel.text = "SAFETY";
            iconContainer.SetActive(true);
        }

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < displayAmount)
            {
                healthIcons[i].sprite = OnSprite;
            }
            else
            {
                healthIcons[i].sprite = offSprite;
            }
        }
    }
}
