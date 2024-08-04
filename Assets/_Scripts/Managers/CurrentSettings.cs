using UnityEngine;

public class CurrentSettings : MonoBehaviour
{
    [SerializeField] SetGridSizeUI _gridSizeUI;

    private int _currentGridSize;

    public int CurrentGridSize { get => _currentGridSize; }
    public static CurrentSettings Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void SetCurrentGridSize()
    {
        if (_gridSizeUI == null) return;
        _currentGridSize = _gridSizeUI.GridSize;
    }
}
