using UnityEngine;
using System.Collections;

public class RestartLevelButton : CustomButton 
{
    protected override void OnButtonClick()
    {
        Director.Instance.LoadLevel(Application.loadedLevelName);
    }
}
