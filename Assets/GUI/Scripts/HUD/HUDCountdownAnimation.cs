using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDCountdownAnimation : CounterAnimation 
{
    void Start()
    {
        base.Awake();

        Time.timeScale = 0;
    }

    public override void Update()
    {
        if (CurrentValue == TargetValue)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
            base.Update();
    }
}
