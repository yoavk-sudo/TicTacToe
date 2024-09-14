using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameButtonsHandler _grid;
    [SerializeField] PlayersUI _playersUI;
    [SerializeField] BarButtons _barButtons;
    [SerializeField] GameObject _pauseMenu;

    private int _xScore;
    private int _oScore;
    private bool _isXTurn = true;
    private int[,] _gameMatrix;
    private int _gridSize;

    public static GameManager Instance { get; private set; }
    public bool IsXTurn { get => _isXTurn; }
    public GameObject PauseMenu { get => _pauseMenu; }


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
        _playersUI.ChangeMarkAccordingToPlayerTurn();
        _gridSize = CurrentSettings.Instance.CurrentGridSize;
        SetUpGame();
    }

    [ContextMenu("Change Turn")]
    public void ChangeTurn()
    {
        _isXTurn = !_isXTurn;
        _playersUI.ChangeMarkAccordingToPlayerTurn();
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
            _playersUI.IncreasePlayerScore(0, _xScore);
        }
        else
        {
            _oScore += 1;
            _playersUI.IncreasePlayerScore(1, _oScore);
        }
    }

    public void TogglePauseGame()
    {
        _grid.ToggleButtonsInteractability();
        _barButtons.ToggleButtonsInteractability();
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
