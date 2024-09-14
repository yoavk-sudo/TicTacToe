using TMPro;
using UnityEngine;

public class PlayersUI : MonoBehaviour
{
    [SerializeField] TMP_Text _xScoreText;
    [SerializeField] TMP_Text _oScoreText;
    [SerializeField] GameObject _xSymbol;
    [SerializeField] GameObject _oSymbol;

    public void IncreasePlayerScore(int playerIndex, int score)
    {
        switch (playerIndex)
        {
            case 0:
                _xScoreText.text = score.ToString();
                break;
            case 1:
                _oScoreText.text = score.ToString();
                break;
        }
    }

    public void ChangeMarkAccordingToPlayerTurn()
    {
        _xSymbol.SetActive(GameManager.Instance.IsXTurn);
        _oSymbol.SetActive(!GameManager.Instance.IsXTurn);
    }
}
