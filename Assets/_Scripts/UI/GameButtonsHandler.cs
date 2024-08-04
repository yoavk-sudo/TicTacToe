using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameButtonsHandler : MonoBehaviour
{
    [SerializeField] List<Button> _cells;
    [SerializeField] Button _gridButton;
    [SerializeField] GridLayoutGroup _grid;

    private void OnValidate()
    {
        _cells = GetComponentsInChildren<Button>().ToList();
        _grid = GetComponent<GridLayoutGroup>();
    }

    public void SetGrid()
    {
        SetGridConstraintCount();
        FillGridWithButtons();
    }

    private void SetGridConstraintCount()
    {
        _grid.constraintCount = CurrentSettings.Instance.CurrentGridSize;
    }

    private void FillGridWithButtons()
    {
        int totalButtonCount = (int)Mathf.Pow(_grid.constraintCount, 2);
        while (_cells.Count > totalButtonCount)
        {
            Button buttonToRemove = _cells[_cells.Count - 1];
            _cells.RemoveAt(_cells.Count - 1);
            Destroy(buttonToRemove.gameObject);
        }
        while (_cells.Count < totalButtonCount)
        {
            Button newButton = Instantiate(_gridButton, _grid.transform);
            _cells.Add(newButton);
        }
    }

    public void ToggleButtonsInteractability()
    {
        foreach (var button in _cells)
        {
            button.interactable = !button.interactable;
        }
    }

    public void GetButton(Button cell)
    {
        int index = _cells.IndexOf(cell);
        CommandManager.Instance.ExecuteCommand(cell, index);
    }
}
