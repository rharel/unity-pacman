using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndgameStatusHeader : MonoBehaviour 
{
    public Color winnerColor;
    public string winnerText;

    public Color loserColor;
    public string loserText;

    private Text display;

	void Awake()
    {
        display = GetComponent<Text>();
    }

    public void UpdateDisplay()
    {
        if (GameManager.GameIsWon)
        {
            display.color = winnerColor;
            display.text = winnerText;
        }
        else
        {
            display.color = loserColor;
            display.text = loserText;
        }
    }
}
