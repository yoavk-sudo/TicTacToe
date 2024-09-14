using UnityEngine;

public class BarButtons : MonoBehaviour
{
    [SerializeField] GameObject _buttons;

    private void OnValidate()
    {
        try
        {
            _buttons = GameObject.Find("Buttons");
        }
        catch (System.Exception)
        {
        }
    }

    public void ToggleButtonsInteractability()
    {
        _buttons.SetActive(!_buttons.activeSelf);
    }
}
