using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldCounterText;
    [SerializeField] private TextMeshProUGUI _waveCounter;

    // Start is called before the first frame update
    void Start()
    {
        _goldCounterText.text = "Gold: " + 0; //replace with variables from gold counter script later
        _waveCounter.text = "Wave " + 1 + " of " + 5; //replace with variables from wave counter logic script later
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
