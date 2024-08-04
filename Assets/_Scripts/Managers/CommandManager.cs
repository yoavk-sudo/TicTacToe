using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    private List<string> _commands;

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

    public void Undo()
    {
        //if successful, change turns
    }

    public void Redo()
    {
        //if successful, change turns

    }

    public void ExecuteCommand(Button cell, int index)
    {
        cell.interactable = false;
        GameManager.Instance.ChangeMark(index);
        GameManager.Instance.IsGameWon();
    }
}
