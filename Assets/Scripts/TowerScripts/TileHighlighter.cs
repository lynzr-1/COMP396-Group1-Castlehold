using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    private Renderer highlightRenderer;

    private void Awake()
    {
        highlightRenderer = GetComponent<Renderer>();
        if (highlightRenderer == null)
        {
            Debug.LogError("Renderer component is missing from TileHighlighter.");
        }

        gameObject.SetActive(false); // Hide initially
    }
    // Enable and position the highlight
    public void ShowHighlight(Vector3 position, bool isValid)
    {
        Debug.Log("Showing highlight at position: " + position + ", valid: " + isValid);
        position.y = 0.21f;
        transform.position = position;
        highlightRenderer.material.color = isValid ? validColor : invalidColor;
        gameObject.SetActive(true);
    }

    public void HideHighlight()
    {
        gameObject.SetActive(false);
    }
}