using UnityEngine;
using UnityEngine.UI;

public abstract class CustomButton : MonoBehaviour
{
    private Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    protected abstract void OnButtonClick();
}