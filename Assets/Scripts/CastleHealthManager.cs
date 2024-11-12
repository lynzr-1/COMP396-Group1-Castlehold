using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Settings")]
    public Image healthFillImage;  //health bar fill image assigned in inspector
    public TextMeshProUGUI healthPercentage;

    private void Start()
    {
        currentHealth = maxHealth;
        healthPercentage.text = $"{currentHealth}%";
        UpdateHealthBar();
    }

    // Function to take damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //health shouldn't go below zero or above max health
        healthPercentage.text = $"{currentHealth}%";
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Debug.Log("Castle has been destroyed!");
            // game over logic can go here
        }
    }

    // Function to update the health bar UI
    private void UpdateHealthBar()
    {
        healthFillImage.fillAmount = currentHealth / maxHealth;
        healthPercentage.text = $"{currentHealth}%";
    }
}
