using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonsHandler : MonoBehaviour
{
    [SerializeField] List<Button> _cells;
    [SerializeField] Button _gridButton;
    [SerializeField] GridLayoutGroup _grid;

    [Header("Sprites")]
    [SerializeField] Sprite _defaultButtonSprite;
    [SerializeField] Sprite _player1MarkSprite;
    [SerializeField] Sprite _player2MarkSprite;

    private bool _isSubscribedToCommandManagerOnEnable = false;

    public List<Button> Cells { get => _cells; private set => _cells = value; }

    private void OnValidate()
    {
        Cells = GetComponentsInChildren<Button>().ToList();
        _grid = GetComponent<GridLayoutGroup>();
    }

    private void OnEnable()
    {
        if (CommandManager.Instance == null)
        {
            _isSubscribedToCommandManagerOnEnable = false;
            return;
        }
        CommandManager.Instance.OnUndo += HandleUndoSpriteChange;
        CommandManager.Instance.OnRedo += HandleRedoSpriteChange;
        _isSubscribedToCommandManagerOnEnable = true;
    }

    private void OnDisable()
    {
        CommandManager.Instance.OnUndo -= HandleUndoSpriteChange;
        CommandManager.Instance.OnRedo -= HandleRedoSpriteChange;
    }

    private void Start()
    {
        if(!_isSubscribedToCommandManagerOnEnable && CommandManager.Instance != null)
        {
            CommandManager.Instance.OnUndo += HandleUndoSpriteChange;
            CommandManager.Instance.OnRedo += HandleRedoSpriteChange;
        }
    }

    public void SetGrid()
    {
        SetGridConstraintCount();
        FillGridWithButtons();
        foreach (var cell in Cells)
        {
            ChangeButtonImageToDefault(cell);
            cell.interactable = true;
        }
    }

    private void SetGridConstraintCount()
    {
        _grid.constraintCount = CurrentSettings.Instance.CurrentGridSize;
    }

    private void FillGridWithButtons()
    {
        int totalButtonCount = (int)Mathf.Pow(_grid.constraintCount, 2);
        while (Cells.Count > totalButtonCount)
        {
            Button buttonToRemove = Cells[Cells.Count - 1];
            Cells.RemoveAt(Cells.Count - 1);
            Destroy(buttonToRemove.gameObject);
        }
        while (Cells.Count < totalButtonCount)
        {
            Button newButton = Instantiate(_gridButton, _grid.transform);
            newButton.interactable = true;
            Cells.Add(newButton);
        }
    }

    public void ToggleButtonsInteractability()
    {
        foreach (var button in Cells)
        {
            button.interactable = !button.interactable;
        }
    }

    public void GetButton(Button cell)
    {
        int index = Cells.IndexOf(cell);
        ChangeButtonImageToPlayerMark(cell);
        CommandManager.Instance.ExecuteCommand(cell, index);
    }

    private void ChangeButtonImageToPlayerMark(Button cell)
    {
        if (GameManager.Instance.IsXTurn)
            ChangeButtonImage(cell, _player1MarkSprite);
        else
            ChangeButtonImage(cell, _player2MarkSprite);
    }

    private void ChangeButtonImageToDefault(Button cell)
    {
        ChangeButtonImage(cell, _defaultButtonSprite);
    }

    private void HandleUndoSpriteChange(Button button)
    {
        ChangeButtonImage(button, _defaultButtonSprite);
    }

    private void HandleRedoSpriteChange(Button button)
    {
        if (GameManager.Instance.IsXTurn) 
            ChangeButtonImage(button, _player1MarkSprite);
        else
            ChangeButtonImage(button, _player2MarkSprite);
    }

    private void ChangeButtonImage(Button cell, Sprite sprite)
    {
        cell.image.sprite = sprite;
    }
}
