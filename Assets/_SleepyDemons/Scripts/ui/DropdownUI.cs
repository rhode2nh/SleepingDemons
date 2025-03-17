using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DropdownUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _distanceToClose;
    [SerializeField] private DropdownOptionUI _dropdownOptionUI;
    [SerializeField] private Vector2 _offset;
    
    private RectTransform _rectTransform;
    private ParentObject _optionsParent;

    private Slot _slot;
    private List<DropdownOptionUI> _dropdownOptions;
    private Vector2 _cursorReferencePos;
    private Vector2 _currentCursorPos;
    private bool _cursorIsInArea;
    private bool _isOpen;

    private void Awake()
    {
        _dropdownOptions = new List<DropdownOptionUI>();
        _rectTransform = GetComponent<RectTransform>();
        _optionsParent = GetComponentInChildren<ParentObject>();
    }

    private void Start()
    {
        Close();
    }

    private void Update()
    {
        UpdateCursorPosition();
        CheckCursorInArea();
        CheckDistanceToMouse();
    }

    public bool IsOpen()
    {
        return _isOpen;
    }

    public void Open(Slot slot)
    {
        if (!_isOpen)
        {
            gameObject.SetActive(true);
            _slot = slot;
            _isOpen = true;
            foreach (var option in slot.Item.Options)
            {
                var dropDownOption = Instantiate(_dropdownOptionUI, _optionsParent.transform, true);
                dropDownOption.Initialize(slot, this, option);
                _dropdownOptions.Add(dropDownOption);
            }
            FilterOptions();
        }
        MoveToMouse();
    }

    private void FilterOptions()
    {
        if (InventoryManager.instance.selectedSlot.GUID == _slot.GUID)
        {
            var index = _dropdownOptions.FindIndex(x => x.GetLabel() == "Equip");
            if (index > 0)
            {
                _dropdownOptions[index]?.gameObject.SetActive(false);
            }
        }
        else
        {
            var index = _dropdownOptions.FindIndex(x => x.GetLabel() == "Unequip");
            if (index > 0)
            {
                _dropdownOptions[index]?.gameObject.SetActive(false);
            }
        }
    }

    private void MoveToMouse()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition,
            _canvas.worldCamera, out _cursorReferencePos);
        Vector3 mousePos = _canvas.transform.TransformPoint(_cursorReferencePos + _offset);
        transform.position = mousePos;
    }

    private void CheckCursorInArea()
    {
        _cursorIsInArea = RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, Input.mousePosition);
    }

    private void CheckDistanceToMouse()
    {
        if (!_isOpen) return;
        if (_cursorIsInArea) return;

        if (Vector2.Distance(_cursorReferencePos, _currentCursorPos) >= _distanceToClose)
        {
            Close();
        }
    }

    private void UpdateCursorPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition,
            _canvas.worldCamera, out _currentCursorPos);
    }
    
    public void Close()
    {
        foreach (var dropdownOption in _dropdownOptions)
        {
            Destroy(dropdownOption.gameObject);
        }
        _dropdownOptions.Clear();
        gameObject.SetActive(false);
        _isOpen = false;
    }
}
