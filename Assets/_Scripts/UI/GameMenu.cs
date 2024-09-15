using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject _resumeButton;

    public void HideResumeButton()
    {
        _resumeButton.SetActive(false);
    }

    public void RevealResumeButton()
    {
        _resumeButton.SetActive(true);
    }

    public void ToggleGameMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
