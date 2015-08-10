using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// The director is responsible for loading and transitioning between levels (and menus).
/// </summary>
public class Director : Singleton<Director> 
{
    public float levelLoadDelay = 1;

    public static event Action LevelLoadStart = delegate { };
    public static event Action LevelLoadEnd = delegate { };

    private static readonly string LEVEL_MAIN_MENU = "main_menu";

    private AsyncOperation levelLoadingOperation;

    public void LoadMainMenu()
    {
        LoadLevel(LEVEL_MAIN_MENU);
    }

    public void LoadLevel(string levelName)
    {
        LevelLoadStart();
        StartCoroutine(LoadLevelDelayed(levelName));
    }

    public IEnumerator LoadLevelDelayed(string levelName)
    {
        yield return new WaitForSeconds(levelLoadDelay);
        levelLoadingOperation = Application.LoadLevelAsync(levelName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (levelLoadingOperation != null && levelLoadingOperation.isDone)
        {
            levelLoadingOperation = null;
            LevelLoadEnd();
        }
    }
}
