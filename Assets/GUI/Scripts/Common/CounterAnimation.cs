using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CounterAnimation : MonoBehaviour 
{
    public int initialValue = 0;
    public int initialTarget = 0;
    public float incrementSpeed = 1;

    public int TargetValue { get; set; }
    public float CurrentValue { get; set; }

    private Text display;

    protected virtual void Awake()
    {
        display = GetComponent<Text>();
        display.text = initialValue.ToString();

        CurrentValue = initialValue;
        TargetValue = initialTarget;
    }

    public virtual void Update()
    {
        if (CurrentValue == TargetValue)
            return;

        float difference = TargetValue - CurrentValue;
        float sign = Mathf.Sign(difference);
        float increment = sign * incrementSpeed * Time.unscaledDeltaTime;
        CurrentValue += increment;
   
        if ((sign > 0 && CurrentValue > TargetValue) ||
            (sign < 0 && CurrentValue < TargetValue))
        {
            CurrentValue = TargetValue;
        }

        display.text = Mathf.RoundToInt(CurrentValue).ToString();
    }
}
