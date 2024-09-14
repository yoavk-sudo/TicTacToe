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
    public bool IsXTurn { get => _isXTurn; }

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
    public void ChangeTurn()
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
        CommandManager.Instance.ResetCommandList();
    }

    public void IsGameWon()
    {
        if(CheckWinnerHorizontal() || CheckWinnerVertical() || CheckWinnerDiagonal())
        {
            //if won
            IncreaseScore();
            TogglePauseGame(); //not good enough, as it will set selected buttons as interactable
            return;
        }
        if(CheckForTie())
        {
            TogglePauseGame(); //not good enough, as it will set selected buttons as interactable
            return;
        }
        //if not won
        ChangeTurn();
    }

    private bool CheckForTie()
    {
        for (int i = 0; i < _gridSize; i++)
        {
            for (int j = 0; j < _gridSize; j++)
            {
                if (_gameMatrix[i, j] == 0) // check if there is a cell not occupied by a player
                    return false;
            }
        }
        return true;
    }

    private bool CheckWinnerDiagonal()
    {
        if (_gridSize % 2 == 0) return false; //no diagonals in an even base matrix

        int firstPlayerMarks = 0;
        int secondPlayerMarks = 0;
        //check from top left, to bottom right
        for (int i = 0; i < _gridSize; i++)
        {
            CountPlayerMarks(i, ref firstPlayerMarks, ref secondPlayerMarks, i);
        }
        if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
        {
            Debug.Log("Won diagonal from top");
            return true;
        }

        //check from bottom left, to top right
        firstPlayerMarks = 0;
        secondPlayerMarks = 0;
        int j = _gridSize - 1;
        for (int i = 0; i < _gridSize; i++)
        {
            CountPlayerMarks(i, ref firstPlayerMarks, ref secondPlayerMarks, j);
            j--;
        }
        if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
        {
            Debug.Log("Won diagonal from bottom");
            return true;
        }
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
                CountPlayerMarks(j, ref firstPlayerMarks, ref secondPlayerMarks, i);
            }
            if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
            {
                Debug.Log("Won vertical, i: " + i);
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
                CountPlayerMarks(i, ref firstPlayerMarks, ref secondPlayerMarks, j);
            }
            if (firstPlayerMarks == _gridSize || secondPlayerMarks == _gridSize)
            {
                Debug.Log("Won horizontal, i: " + i);
                return true;
            }
        }
        return false;
    }

    private void CountPlayerMarks(int i, ref int firstPlayerMarks, ref int secondPlayerMarks, int j)
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
