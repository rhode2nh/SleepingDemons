using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class DropdownOptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Color _outlineHoverColor;
    
    private TMP_Text _label;
    private Outline _outline;
    private Color _outlineColor;
    private Slot _slot;
    private DropdownUI _dropdownUI;
    private DropdownOption _dropdownOption;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outlineColor = _outline.effectColor;
        _label = GetComponentInChildren<TMP_Text>();
    }

    public string GetLabel()
    {
        return _label.text;
    }

    public void Initialize(Slot slot, DropdownUI dropdownUI, DropdownOption dropdownOption)
    {
        _slot = slot;
        _dropdownUI = dropdownUI;
        _dropdownOption = dropdownOption;
        _label.SetText(_dropdownOption.Label);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _outline.effectColor = _outlineHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outline.effectColor = _outlineColor;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _dropdownOption.Process(_slot);
        _dropdownUI.Close();
    }
}
