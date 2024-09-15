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
        if (TryToGetGridSize())
        {
            _currentGridSize = _gridSizeUI.GridSize;
        }
    }
    public bool TryToGetGridSize()
    {
        if (_gridSizeUI == null)
        {
            try
            {
                _gridSizeUI = GameObject.Find("Settings").GetComponent<SetGridSizeUI>();
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        return true;
    }
}
