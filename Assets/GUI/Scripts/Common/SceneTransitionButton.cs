using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionButton : CustomButton 
{
    public string targetLevel;

    protected override void OnButtonClick()
    {
        Director.Instance.LoadLevel(targetLevel);
    }
}
