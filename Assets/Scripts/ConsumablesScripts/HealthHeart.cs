using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthHeart : MonoBehaviour
{
    public int healAmount;
    public GameObject heartTextPrefab;
    private GameObject heartTextInstance; //instance of the TextMeshPro object

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (heartTextPrefab != null)
        {
            heartTextInstance = Instantiate(heartTextPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            UpdateHeartText();
        }

        HealCastle();
    }

    public void SetHealAmount(int amount)
    {
        healAmount = amount;
        UpdateHeartText();
    }

    private void UpdateHeartText()
    {
        if (heartTextInstance != null)
        {
            TextMeshPro heartText = heartTextInstance.GetComponent<TextMeshPro>();
            if (heartText != null)
            {
                heartText.text = "+" + healAmount.ToString() + " hp";
            }
        }
    }

    private void Update()
    {
        if (heartTextInstance != null)
        {
            heartTextInstance.transform.position = transform.position + Vector3.up * 1.5f;
            heartTextInstance.transform.rotation = Quaternion.identity; // Ensure the text is always upright
        }
    }

    private void OnDestroy()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (heartTextInstance != null)
        {
            Destroy(heartTextInstance);
        }
    }

    private void HealCastle()
    {
        // Find the CastleHealthManager and call HealCastle
        CastleHealthManager castleHealthManager = FindObjectOfType<CastleHealthManager>();
        if (castleHealthManager != null)
        {
            castleHealthManager.HealCastle(healAmount); // Call the HealCastle function
            Debug.Log($"Healed castle for {healAmount} HP");
        }
        else
        {
            Debug.LogError("CastleHealthManager not found in the scene!");
        }
    }
}

