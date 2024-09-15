using UnityEngine;
using TMPro;

public class SetGridSizeUI : MonoBehaviour
{
    [SerializeField] int _maxGridSize = 5;
    [SerializeField] int _minGridSize = 3;
    [SerializeField] TMP_Text _rowSizeText;
    [SerializeField] TMP_Text _colSizeText;

    private int _currentRowSize = 3;
    private int _currentColSize = 3;

    public int GridSize { get { return _currentRowSize == _currentColSize ? _currentRowSize : _minGridSize; } }

    private void Start()
    {
        InitializeGridSize();
    }

    private void InitializeGridSize()
    {
        if (!int.TryParse(_rowSizeText.text, out _currentRowSize))
        {
            _currentRowSize = _minGridSize;
        }
        if (!int.TryParse(_colSizeText.text, out _currentColSize))
        {
            _currentColSize = _minGridSize;
        }
        _currentRowSize = Mathf.Clamp(_currentRowSize, _minGridSize, _maxGridSize);
        _currentColSize = Mathf.Clamp(_currentColSize, _minGridSize, _maxGridSize);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _rowSizeText.text = _currentRowSize.ToString();
        _colSizeText.text = _currentColSize.ToString();
    }

    public void IncreaseGridSize()
    {
        if (_currentRowSize < _maxGridSize && _currentColSize < _maxGridSize)
        {
            _currentRowSize++;
            _currentColSize++;
            UpdateUI();
        }
    }

    public void DecreaseGridSize()
    {
        if (_currentRowSize > _minGridSize && _currentColSize > _minGridSize)
        {
            _currentRowSize--;
            _currentColSize--;
            UpdateUI();
        }
    }

    public void GetGridSize()
    {
        CurrentSettings.Instance.SetCurrentGridSize();
    }
}
