using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private GameObject towerPreview;
    public GameObject towerPrefab;
    public TileHighlighter tileHighlighter;
    private bool isValidPlacement = false;
    private Vector3 tilePosition;

    private void Start()
    {
        //tileHighlighter = FindObjectOfType<TileHighlighter>();
        if (tileHighlighter == null)
        {
            Debug.LogError("TileHighlighter not found in the scene!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        towerPreview = Instantiate(towerPrefab, GetMouseWorldPosition(), Quaternion.identity);
        towerPreview.GetComponent<Collider>().enabled = false; // Disable collisions for preview
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (towerPreview != null)
        {
            Vector3 position = GetMouseWorldPosition();
            towerPreview.transform.position = position;

            // Check if the current position is valid and update the highlight
            isValidPlacement = CheckValidPlacement(position);
            tileHighlighter.ShowHighlight(tilePosition, isValidPlacement);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isValidPlacement)
        {
            Instantiate(towerPrefab, tilePosition, Quaternion.identity);
        }
        Destroy(towerPreview);
        tileHighlighter.HideHighlight();
    }

    private bool CheckValidPlacement(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("ValidTowerPlacementTiles");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("ValidTowerPlacement"))
            {
                tilePosition = hit.transform.position; // Save the tile position for snapping
                Debug.Log("Tile position detected: " + tilePosition);
                return true;
            }
        }
        return false;
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 20; // Adjust Z offset as needed
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
