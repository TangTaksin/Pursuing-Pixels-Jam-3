using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject iconContainer;
    public Image[] healthIcons;
    public Sprite OnSprite, offSprite;

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        var displayAmount = maxHealth - currentHealth;

        if (displayAmount <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
            gameObject.SetActive(true);

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
