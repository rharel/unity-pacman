using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitButton : CustomButton 
{
    protected override void OnButtonClick()
    {
        Director.Instance.Quit();
    }
}
