using UnityEngine;
using System.Collections;

public class PausePanel : MonoBehaviour 
{
	void Awake()
    {
        GameManager.Pause += OnPause;
        GameManager.Resume += OnResume;

        gameObject.SetActive(false);
	}
	
    void OnDestroy()
    {
        GameManager.Pause -= OnPause;
        GameManager.Resume -= OnResume;
    }

	private void OnPause()
    {
        if (!GameManager.GameHasEnded)
            gameObject.SetActive(true);
    }

    private void OnResume()
    {
        gameObject.SetActive(false);
    }
}
