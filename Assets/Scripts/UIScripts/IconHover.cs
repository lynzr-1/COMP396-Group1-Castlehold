using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string towerName;
    public string range;
    public string damage;
    public string attackSpeed;
    public ToolTipManager tooltipManager;

    private void Start()
    {
        //tooltipManager = FindObjectOfType<ToolTipManager>();
        //if (tooltipManager == null)
        //{
        //    Debug.LogError("TooltipManager not found in the scene.");
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show tooltip with tower information
        Vector3 iconPosition = transform.position;
        tooltipManager.ShowToolTip(towerName, range, damage, attackSpeed, iconPosition);
        Debug.Log("Tooltip displayed");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide tooltip when not hovering
        tooltipManager.HideToolTip();
    }
}
