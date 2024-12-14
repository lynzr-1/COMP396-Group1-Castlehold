using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private PlayerGoldManager _playerGoldManager; //assign the gold manager script

    private GameObject towerPreview;
    private bool isValidPlacement = false;
    private Vector3 tilePosition;
    private bool canPlace = false; //for gold check
    private bool isPlacingTower = false; //to avoid duplicate placements

    public TileHighlighter tileHighlighter;

    [Header("Tower Details")]
    public GameObject towerPrefab;
    public int towerCost = 0;
    

    private void Start()
    {
        if (_playerGoldManager == null)
        {
            _playerGoldManager = FindObjectOfType<PlayerGoldManager>();
        }

        if (tileHighlighter == null)
        {
            Debug.LogError("TileHighlighter not found in the scene!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlacingTower) return; //to avoid duplicate placements

        if (_playerGoldManager != null && _playerGoldManager.GetGold() >= towerCost)
        {
            towerPreview = Instantiate(towerPrefab, GetMouseWorldPosition(), Quaternion.identity); //instantiate a preview of the tower
            towerPreview.GetComponent<Collider>().enabled = false; //disable collisions for preview
            canPlace = true; //has enough gold so can place the tower
            isPlacingTower = true;
        }
        else 
        {
            Debug.LogWarning("Not enough gold to purchase this tower!");
            canPlace = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isPlacingTower || !canPlace || towerPreview == null) return;

        Vector3 position = GetMouseWorldPosition();
        towerPreview.transform.position = position;

        //check if the current position is valid and update the highlight
        isValidPlacement = CheckValidPlacement(position);
        tileHighlighter.ShowHighlight(tilePosition, isValidPlacement);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isPlacingTower || towerPreview == null) return;

        if (canPlace && isValidPlacement)
        {
            if (_playerGoldManager != null && _playerGoldManager.SpendGold(towerCost)) // Subtract gold from the player
            {
                Instantiate(towerPrefab, tilePosition, Quaternion.identity); // Place the tower
            }
            else
            {
                Debug.LogWarning("Failed to spend gold.");
            }
        }
        else
        {
            Debug.Log("Invalid placement or insufficient gold.");
        }

        Cleanup(); //call clean up function to destroy the preview object and reset variables
    }

    private void Cleanup()
    {
        if (towerPreview != null)
        {
            Destroy(towerPreview); // Destroy the preview object
        }
        tileHighlighter.HideHighlight(); // Hide any highlights
        isPlacingTower = false;
        canPlace = false;
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
                tilePosition = hit.transform.position; //save the tile position for snapping
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
