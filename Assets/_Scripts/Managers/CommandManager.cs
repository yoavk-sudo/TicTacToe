using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    public event Action<Button> OnUndo;
    public event Action<Button> OnRedo;

    private List<Command> _commands;
    private int _indexOfCurrentCommand;

    public static CommandManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        _commands = new();
    }

    public void Undo()
    {
        if (_commands == null || _commands.Count == 0 || _indexOfCurrentCommand < 0) 
            return;
        //_commands[_indexOfCurrentCommand].GetButton.interactable = !_commands[_indexOfCurrentCommand].GetButton.interactable;
        _commands[_indexOfCurrentCommand].Unexecute();
        OnUndo?.Invoke(_commands[_indexOfCurrentCommand].GetButton);
        ChangeCommandIndexByParameter(-1);
        GameManager.Instance.ChangeTurn();
    }

    public void Redo()
    {
        if (_commands == null || _commands.Count == 0 || _indexOfCurrentCommand + 1 >= _commands.Count) 
            return;
        _commands[_indexOfCurrentCommand + 1].Execute();
        OnRedo?.Invoke(_commands[_indexOfCurrentCommand].GetButton);
        ChangeCommandIndexByParameter(1);
        GameManager.Instance.ChangeTurn();
    }

    private void ChangeCommandIndexByParameter(int additive)
    {
        _indexOfCurrentCommand += additive;
        _indexOfCurrentCommand = Mathf.Clamp(_indexOfCurrentCommand, 0, _commands.Count - 1);
    }

    public void ExecuteCommand(Button cell, int index)
    {
        Command newCommand = CreateAndAddNewCommand(cell);
        newCommand.Execute();
        _indexOfCurrentCommand = _commands.Count - 1;
        GameManager.Instance.ChangeMark(index);
        GameManager.Instance.IsGameWon();
    }

    private Command CreateAndAddNewCommand(Button cell)
    {
        Command newCommand = new();
        newCommand.GetButton = cell;
        while (_commands.Count > _indexOfCurrentCommand + 1)
        {
            _commands.RemoveAt(_commands.Count - 1);
        }
        _commands.Add(newCommand);
        return newCommand;
    }

    public void ResetCommandList()
    {
        _commands.Clear();
        _indexOfCurrentCommand = 0;
    }
}
