using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IUIPanel
{
    [SerializeField] private ItemUI _itemUI;
    [SerializeField] private ItemPreview _itemPreview;
    
    private List<ItemUI> _itemUIList;
    private ParentObject _itemsParent;
    [SerializeField] private DropdownUI _dropdownUI;

    private void Awake()
    {
        _itemsParent = GetComponentInChildren<ParentObject>();
        _dropdownUI = GetComponentInChildren<DropdownUI>();
        _itemUIList = new List<ItemUI>();
    }

    private void Start()
    {
        UIManager.Instance.OnOpenInventoryUI += OpenPanel;
        UIManager.Instance.OnCloseInventoryUI += ClosePanel;
        InventoryManager.instance.OnInitializeInventoryGUI += InitializeSlots;
        InventoryManager.instance.OnUpdateInventoryGUI += UpdateUI;
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < InventoryManager.instance.inventory.MaxLength; i++)
        {
            var instantiatedItemUI = Instantiate(_itemUI, _itemsParent.transform, true);
            instantiatedItemUI.Initialize(InventoryManager.instance.inventory.Get(i), _dropdownUI, _itemPreview);
            _itemUIList.Add(instantiatedItemUI);
        }
        
        ClosePanel();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < InventoryManager.instance.inventory.MaxLength; i++)
        {
            _itemUIList[i].Initialize(InventoryManager.instance.inventory.Get(i), _dropdownUI, _itemPreview);
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
}
