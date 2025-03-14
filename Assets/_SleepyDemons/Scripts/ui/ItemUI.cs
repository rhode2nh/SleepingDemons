using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Outline _outline;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Color _emptyTextColor;
    [SerializeField] private Color _normalTextColor;
    [SerializeField] private Color _outlineHoverColor;
    [SerializeField] private Color _outlineColor;

    private Slot _slot;
    private DropdownUI _dropdownUI;
    private ItemPreview _itemPreview;

    public void Initialize(Slot slot, DropdownUI dropdownUI, ItemPreview itemPreview)
    {
        _slot = slot;
        _dropdownUI = dropdownUI;
        _itemPreview = itemPreview;
        if (_slot.Item == DatabaseManager.instance.GetEmptyItem())
        {
            _title.SetText("Empty");
            _title.fontStyle = FontStyles.Italic;
            _title.color = _emptyTextColor;
        }
        else
        {
            _title.SetText(slot.Name);
            _title.fontStyle = FontStyles.Normal;
            _title.color = _normalTextColor;
        }

        _outline.effectColor = _outlineColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_slot.Item == DatabaseManager.instance.GetEmptyItem() || _dropdownUI.IsOpen()) return;
        
        _outline.effectColor = _outlineHoverColor;
        _itemPreview.Open(_slot.Item.MarketItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outline.effectColor = _outlineColor;
        if (!_dropdownUI.IsOpen())
        {
            _itemPreview.Close();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right &&
            _slot.Item != DatabaseManager.instance.GetEmptyItem())
        {
            _dropdownUI.Open(_slot);
        }
    }
}