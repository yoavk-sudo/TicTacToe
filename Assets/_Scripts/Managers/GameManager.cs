using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameButtonsHandler _grid;
    [SerializeField] TMP_Text _xScoreText;
    [SerializeField] TMP_Text _oScoreText;
    [SerializeField] GameObject _buttons;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _xSymbol;
    [SerializeField] GameObject _oSymbol;

    private int _xScore;
    private int _oScore;
    private bool _isXTurn = true;
    private int[,] _gameMatrix;
    private int _gridSize;



    public static GameManager Instance { get; private set; }

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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ChangeTurnSymbol();
        _gridSize = CurrentSettings.Instance.CurrentGridSize;
        SetUpGame();
    }

    [ContextMenu("Change Turn")]
    private void ChangeTurn()
    {
        _isXTurn = !_isXTurn;
        ChangeTurnSymbol();
    }
    private void ChangeTurnSymbol()
    {
        _xSymbol.SetActive(_isXTurn);
        _oSymbol.SetActive(!_isXTurn);
    }

    public void SetUpGame()
    {
        _grid.SetGrid();
        _gameMatrix = new int[_gridSize, _gridSize];
    }

    public void IsGameWon()
    {
        if(CheckWinnerHorizontal() || CheckWinnerVertical() || CheckWinnerDiagonal())
        {
            //if won
            IncreaseScore();
            TogglePauseGame(); //not good enough, as it will set selected buttons as interatable
            return;
        }
        //if not won
        ChangeTurn();
    }

    private bool CheckWinnerDiagonal()
    {
        if (_gridSize % 2 == 0) return false; //no diagonals in an even base matrix

        int firstPlayerMarks = 0;
        int secondPlayerMarks = 0;
        //check from top left, to bottom right
        for (int i = 0; i < _gridSize; i++)
        {
            switch (_gameMatrix[i,i])
            {
                case 0:
                    break;
                case 1:
                    firstPlayerMarks++;
                    break;
                case 2:
                    secondPlayerMarks++;
                    break;
                default:
                    break;
            }
        }
        if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
            return true;

        //check from bottom left, to top right
        firstPlayerMarks = 0;
        secondPlayerMarks = 0;
        int j = _gridSize - 1;
        for (int i = 0; i < _gridSize; i++)
        {
            switch (_gameMatrix[i, j])
            {
                case 0:
                    break;
                case 1:
                    firstPlayerMarks++;
                    break;
                case 2:
                    secondPlayerMarks++;
                    break;
                default:
                    break;
            }
            j--;
        }
        if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
            return true; 
        return false;
    }

    private bool CheckWinnerVertical()
    {
        for (int i = 0; i < _gridSize; i++)
        {
            int firstPlayerMarks = 0;
            int secondPlayerMarks = 0;
            for (int j = 0; j < _gridSize; j++)
            {
                switch (_gameMatrix[j, i])
                {
                    case 0:
                        break;
                    case 1:
                        firstPlayerMarks++;
                        break;
                    case 2:
                        secondPlayerMarks++;
                        break;
                    default:
                        break;
                }
                if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
                    return true;
            }
        }
        return false;
    }

    private bool CheckWinnerHorizontal()
    {
        for (int i = 0; i < _gridSize; i++)
        {
            int firstPlayerMarks = 0;
            int secondPlayerMarks = 0;
            for (int j = 0; j < _gridSize; j++)
            {
                switch (_gameMatrix[i, j])
                {
                    case 0:
                        break;
                    case 1:
                        firstPlayerMarks++;
                        break;
                    case 2:
                        secondPlayerMarks++;
                        break;
                    default:
                        break;
                }
                if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
                    return true;
            }
        }
        return false;
    }

    public void IncreaseScore()
    {
        if (_isXTurn)
        {
            _xScore += 1;
            _xScoreText.text = _xScore.ToString();
        }
        else
        {
            _oScore += 1;
            _oScoreText.text = _oScore.ToString();
        }
    }

    public void TogglePauseGame()
    {
        _grid.ToggleButtonsInteractability();
        _buttons.SetActive(!_buttons.activeSelf);
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }

    public void ChangeMark(int index)
    {
        int row = index / _gridSize;
        int col = index % _gridSize;
        if(_isXTurn)
            _gameMatrix[row, col] = 1;
        else
            _gameMatrix[row, col] = 2;
    }
}
