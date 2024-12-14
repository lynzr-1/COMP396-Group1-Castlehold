using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [Header("Gold Drop")]
    public int minGold;
    public int maxGold;    
    [Range(0, 1)] public float goldDropChance = 0.10f; //base drop chance set to 10%

    [Header("Gold Prefab")]
    public GameObject goldCoinPrefab;

    [Header("Health Drops")]
    public GameObject healthHeartPrefab;
    [Range(0, 1)] public float heartDropChance = 0.10f; //base drop chance set to 10%

    [Header("Other Settings")]
    public Transform dropPosition;
    public Transform heartDropPosition;

    private PlayerGoldManager goldManager;

    private void Start()
    {
        // Find the PlayerGoldManager in the scene
        goldManager = FindObjectOfType<PlayerGoldManager>();

        if (goldManager == null)
        {
            Debug.LogError("PlayerGoldManager not found in the scene!");
        }
    }

    public void DropLoot() 
    {
        TryDropGold();
        TryDropHeart();
    }

    private void TryDropGold()
    {
        if (Random.value <= goldDropChance)  //check if the enemy drops gold
        {
            int goldAmount = Random.Range(minGold, maxGold + 1);  //random gold amount in range
            SpawnGold(goldAmount);

            if (goldManager != null)
            {
                goldManager.AddGold(goldAmount);
                Debug.Log($"Added {goldAmount} gold to player.");
            }
        }
    }

    private void SpawnGold(int amount)
    {
        if (goldCoinPrefab != null)
        {
            GameObject gold = Instantiate(goldCoinPrefab, dropPosition != null ? dropPosition.position : transform.position, Quaternion.identity);

            //pass the gold amount to the gold prefab script
            GoldCoin goldScript = gold.GetComponent<GoldCoin>();
            if (goldScript != null)
            {
                goldScript.SetGoldAmount(amount); // Update the gold prefab's visuals or behavior
            }

            Destroy(gold, 5f); // Destroy the gold prefab after 5 seconds
        }
    }

    private void TryDropHeart()
    {
        if (Random.value <= heartDropChance)  //check if the enemy drops a heart
        {
            SpawnHeart();
        }
    }

    private void SpawnHeart()
    {
        if (healthHeartPrefab != null)
        {
            GameObject heart = Instantiate(healthHeartPrefab, dropPosition != null ? dropPosition.position : transform.position, Quaternion.identity);

            Destroy(heart, 5f); // Destroy the gold prefab after 5 seconds
        }
    }
}
