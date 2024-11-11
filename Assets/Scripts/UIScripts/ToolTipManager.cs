using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public Vector3 offset;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is missing!");
        }
        else { Debug.Log("RectTransform is assigned"); }
        gameObject.SetActive(false); //hide on start
    }

    // Update is called once per frame
    public void ShowToolTip(string towerName, string range, string damage, string attackSpeed, Vector3 position)
    {
        towerNameText.text = towerName;
        rangeText.text = $"Range: {range}";
        damageText.text = $"Damage: {damage}";
        attackSpeedText.text = $"Attack Speed: {attackSpeed}";

        rectTransform.position = position + offset;
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    { 
        gameObject.SetActive(false);
    }
}
