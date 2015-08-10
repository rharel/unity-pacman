using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDRespawnDisplay : MonoBehaviour 
{
    private Text display;

    void Awake()
    {
        display = GetComponent<Text>();
        UpdateDisplay();

        GameManager.PlayerInteraction.Death += OnPlayerDeath;
    }

    void OnDestroy()
    {
        if (GameManager.Exists && GameManager.PlayerInteraction != null)
            GameManager.PlayerInteraction.Death -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        if (!GameManager.GameIsLost)
            UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        display.text = GameManager.LivesRemaining.ToString();
    }
}
