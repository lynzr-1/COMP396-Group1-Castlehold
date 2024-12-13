using UnityEngine;
using TMPro;

public class GoldCoin : MonoBehaviour
{
    public int goldAmount;
    public GameObject goldTextPrefab; //to display the amount of gold dropped
    private GameObject goldTextInstance; //instance of the TextMeshPro object

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        //play the coin drop sound
        if (audioSource != null)
        {
            audioSource.Play();
        }

        //spawn the text prefab to display the amount of gold
        if (goldTextPrefab != null)
        {
            // Instantiate the text prefab slightly above the coin
            goldTextInstance = Instantiate(goldTextPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            // Update the text with the gold amount
            UpdateGoldText();
        }
    }

    public void SetGoldAmount(int amount)
    {
        goldAmount = amount;
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        if (goldTextInstance != null)
        {
            // Update the text component
            TextMeshPro goldText = goldTextInstance.GetComponent<TextMeshPro>();
            if (goldText != null)
            {
                goldText.text = "+" + goldAmount.ToString();
            }
        }
    }

    private void Update()
    {
        //to keep the text above the coin and oriented correctly
        if (goldTextInstance != null)
        {
            goldTextInstance.transform.position = transform.position + Vector3.up * 1.5f;
            goldTextInstance.transform.rotation = Quaternion.identity; // Ensure the text is always upright
        }
    }

    private void OnDestroy()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        //destroy the text label
        if (goldTextInstance != null)
        {
            Destroy(goldTextInstance);
        }
    }
}
