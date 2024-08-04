using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
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
    }

    private void Start()
    {
        _commands = new();
    }

    public void Undo()
    {
        if (_commands == null || _commands.Count == 0 || _indexOfCurrentCommand < 0) return;
        _commands[_indexOfCurrentCommand].GetButton.interactable = !_commands[_indexOfCurrentCommand].GetButton.interactable;
        _indexOfCurrentCommand--;
        GameManager.Instance.ChangeTurn();
    }

    public void Redo()
    {
        if (_commands == null || _commands.Count == 0 || _indexOfCurrentCommand + 1 >= _commands.Count) return;
        _commands[_indexOfCurrentCommand + 1].GetButton.interactable = !_commands[_indexOfCurrentCommand + 1].GetButton.interactable;
        _indexOfCurrentCommand++;
        GameManager.Instance.ChangeTurn();
    }

    public void ExecuteCommand(Button cell, int index)
    {
        cell.interactable = false;
        Command newCommand = new();
        newCommand.GetButton = cell;
        while(_commands.Count > _indexOfCurrentCommand + 1)
        {
            _commands.RemoveAt(_commands.Count - 1);
        }
        _commands.Add(newCommand);
        _indexOfCurrentCommand = _commands.Count - 1;
        GameManager.Instance.ChangeMark(index);
        GameManager.Instance.IsGameWon();
    }
}
