using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {

    public string prefix = "";
    public int minDigits = 1;

    private Text display;

    void Awake()
    {
        display = GetComponent<Text>();
    }

	void Update()
    {
        string score = ScoreManager.Score.ToString()
                       .PadLeft(minDigits, '0');
        display.text = prefix + score;
	}
}
