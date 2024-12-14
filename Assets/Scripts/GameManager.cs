using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Data")]
    public int playerGold = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        Debug.Log($"Player gold updated: {playerGold}");
    }

    public void DeductGold(int amount)
    {
        playerGold = Mathf.Max(playerGold - amount, 0);
        Debug.Log($"Player gold updated: {playerGold}");
    }

    public int GetGold()
    {
        return playerGold;
    }
}
