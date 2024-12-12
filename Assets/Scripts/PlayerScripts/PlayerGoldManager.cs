using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{

    [Header("Gold Settings")]
    public int startingGold = 0;
    private int currentGold;

    [SerializeField] private UIManager _uiManager; //reference to the UI manager script

    // Start is called before the first frame update
    void Start()
    {
        currentGold = startingGold;

        if (_uiManager != null)
        {
            _uiManager.UpdateGoldCounter(currentGold);
        }
        else
        {
            Debug.LogError("UIManager not found in the scene!");
        }
    }

    public int GetGold()
    {
        return currentGold;
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount) //check if player has enough gold to make the purchase
        {
            currentGold -= amount;
            _uiManager?.UpdateGoldCounter(currentGold); //update gold counter on the UI
            return true;
        }

        Debug.LogWarning("Not enough gold!");
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        _uiManager?.UpdateGoldCounter(currentGold); //update gold counter on the UI
    }
}