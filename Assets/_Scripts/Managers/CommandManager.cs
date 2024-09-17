using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    public event Action<Button> OnUndo;
    public event Action<Button> OnRedo;

    private List<Command> _commands;
    private bool _isIndexNegative;
    private int _indexOfCurrentCommand = -1;

    public static CommandManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        _commands = new List<Command>();
    }

    public void Undo()
    {
        if (IsCommandListInvalid() || _indexOfCurrentCommand < 0)
            return;
        while (_indexOfCurrentCommand >= _commands.Count)
        {
            _indexOfCurrentCommand--;
        }
        _commands[_indexOfCurrentCommand].Unexecute();
        OnUndo?.Invoke(_commands[_indexOfCurrentCommand].GetButton);
        ChangeCommandIndex(-1);
        GameManager.Instance.ChangeTurn();
        Debug.Log("Index after undo is " + _indexOfCurrentCommand);
    }

    public void Redo()
    {
        while (_indexOfCurrentCommand < 0)
            _indexOfCurrentCommand++;
        if (IsCommandListInvalid() || _indexOfCurrentCommand >= _commands.Count)
            return;
        _commands[_indexOfCurrentCommand].Execute();
        OnRedo?.Invoke(_commands[_indexOfCurrentCommand].GetButton);

        ChangeCommandIndex(1);
        GameManager.Instance.ChangeTurn();
        Debug.Log("Index after redo is " + _indexOfCurrentCommand);
    }

    private void ChangeCommandIndex(int delta)
    {
        _indexOfCurrentCommand += delta;
        //_isIndexNegative = _indexOfCurrentCommand < 0;
        //_indexOfCurrentCommand = Mathf.Clamp(_indexOfCurrentCommand, 0, _commands.Count);
        Debug.Log(_isIndexNegative);
    }


    private bool IsCommandListInvalid()
    {
        return _commands == null || _commands.Count == 0;
    }

    public void ExecuteCommand(Button cell, int index)
    {
        _indexOfCurrentCommand++;
        if (_indexOfCurrentCommand < _commands.Count)
        {
            _commands.RemoveRange(_indexOfCurrentCommand, _commands.Count - _indexOfCurrentCommand);
        }

        Command newCommand = CreateAndAddNewCommand(cell);
        newCommand.Execute();

        //_indexOfCurrentCommand = _commands.Count;
        _isIndexNegative = false;
        GameManager.Instance.ChangeMark(index);
        GameManager.Instance.IsGameWon();
    }

    private Command CreateAndAddNewCommand(Button cell)
    {
        Command newCommand = new Command { GetButton = cell };

        //// Clear any future commands if user executes new command after undo
        //while (_commands.Count > _indexOfCurrentCommand + 1)
        //{
        //    _commands.RemoveAt(_commands.Count - 1);
        //}

        _commands.Add(newCommand);
        return newCommand;
    }

    public void ResetCommandList()
    {
        _commands.Clear();
        _indexOfCurrentCommand = 0;
    }
}
