using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject howToPlayPanel;
    public GameObject creditsPanel;
    public GameObject optionsPanel;

    // Ensure panels are hidden at start
    private void Start()
    {
        HideAllPanels();
    }

    public void StartGame()
    {
        // Load Level 1 scene
        SceneManager.LoadScene("Level1");
    }

    public void ShowHowToPlay()
    {
        HideAllPanels();
        howToPlayPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        HideAllPanels();
        creditsPanel.SetActive(true);
    }

    public void ShowOptions()
    {
        HideAllPanels();
        optionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    private void HideAllPanels()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }
}

