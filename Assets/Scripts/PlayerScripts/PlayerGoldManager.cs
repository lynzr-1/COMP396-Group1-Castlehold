using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{

    [Header("Gold Settings")]
    public int startingGold = 0;
    private int currentGold;

    [SerializeField] private UIManager _uiManager; //reference to the UI manager script

    private void Awake()
    {
        // Ensure UIManager is assigned
        if (_uiManager == null)
        {
            _uiManager = FindObjectOfType<UIManager>();
            if (_uiManager == null)
            {
                Debug.LogError("UIManager not found in the scene! Make sure it's assigned or present.");
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        Initialize(GameManager.Instance?.playerGold > 0 ? GameManager.Instance.playerGold : startingGold);
    }

    public void Initialize(int initialGold)
    {
        currentGold = initialGold;

        // Update UI with current gold
        _uiManager?.UpdateGoldCounter(currentGold);
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
            GameManager.Instance.DeductGold(amount);
            _uiManager?.UpdateGoldCounter(currentGold); //update gold counter on the UI
            return true;
        }

        Debug.LogWarning("Not enough gold!");
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        GameManager.Instance.AddGold(amount);
        LevelManager.Instance.AddToGoldEarned(amount); //notify level manager
        _uiManager?.UpdateGoldCounter(currentGold); //update gold counter on the UI
    }
}
