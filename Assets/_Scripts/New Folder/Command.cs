using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public Button GetButton { get; set; }

    public void Unexecute()
    {
        GetButton.interactable = !GetButton.interactable;
        //change sprite
    }

    public void Execute()
    {
        GetButton.interactable = !GetButton.interactable;
        //change sprite
    }
}
