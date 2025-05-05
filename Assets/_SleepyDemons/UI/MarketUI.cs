using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MarketItem {
    Previous = -1,
    Current = 0,
    Next = 1,
}

public class MarketUI : MonoBehaviour, IUIPanel, IUIInventory
{
    private List<UISlot> uiSlots;
    [SerializeField] private GameObject _uiSlot;
    [SerializeField] private GameObject itemsParent;
    [SerializeField] private MarketStats _marketStats;
    [SerializeField] private Slot _currentSlot;
    [SerializeField] private Transform _marketModelPos;
    [SerializeField] private BumpArrow _arrowLeft;
    [SerializeField] private BumpArrow _arrowRight;
    [SerializeField] private float _degreesPerSecond;
    [SerializeField] private GameObject _marketCamera;
    [SerializeField] private float _viewItemScaleFactor;
    [SerializeField] private Button _buyButton;
    [SerializeField] private float _marketModelLeftRightOffset;
    [SerializeField] private float _marketModelLerpDuration;
    [SerializeField] private float _marketModelMoveSpeed;
    private Inventory _inventory;
    private Market _market;
    private int _index;
    private Quaternion _lastViewItemRotation;
    private GameObject _curMarketModel;
    private GameObject _leftMarketModel;
    private GameObject _rightMarketModel;
    private bool coroutineStarted;
    private float timeElapsed;
    private MarketItem lastMove;

    void Awake()
    {
        uiSlots = new List<UISlot>();
    }

    void Update()
    {
        if (_curMarketModel != null)
        {
            _curMarketModel.transform.GetChild(0).transform.Rotate(0, _degreesPerSecond * Time.deltaTime, 0);
        }
    }

    public void OpenPanel()
    {
        CrosshairManager.Instance.HideCrossHair();
        gameObject.SetActive(true);
        for (int i = 0; i < _inventory.MaxLength; i++)
        {
            var instancedUISlot = Instantiate(_uiSlot);
            instancedUISlot.transform.SetParent(itemsParent.transform, false);
            uiSlots.Add(instancedUISlot.GetComponent<UISlot>());
            if (_inventory.Get(i).Item != DatabaseManager.instance.GetEmptyItem())
            {
                uiSlots[i].SetSlot(_inventory.Get(i).Item.Icon);
                uiSlots[i].OnPointerClickFunc = OnPointerClickFunc;
                uiSlots[i].Index = i;
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }
        _currentSlot = _inventory.Get(_index);
        uiSlots[_index].SetActive(true);
        // _curMarketModel = Instantiate(_currentSlot.Item.MarketItem, _marketModelPos.position, Camera.main.transform.rotation, _marketCamera.transform);
        _curMarketModel.transform.localScale = _curMarketModel.transform.localScale * _viewItemScaleFactor;
        _lastViewItemRotation = _curMarketModel.transform.rotation;
        // _marketStats.UpdateSlot(_currentSlot);
        CheckPrice();
    }
    
    private int mod(int x, int m)
    {
        return (x%m + m)%m;
    }

    public void ClosePanel()
    {
        CrosshairManager.Instance.ShowCrossHair();
        gameObject.SetActive(false);
        _inventory.Clear();
        _index = 0;
        Destroy(_curMarketModel);
        Destroy(_leftMarketModel);
        Destroy(_rightMarketModel);
    }

    public void OnDisable()
    {
        uiSlots.Clear();
        for (int i = 0; i < itemsParent.transform.childCount; i++)
        {
            Destroy(itemsParent.transform.GetChild(i).gameObject);
        }
    }

    public void SetMarket(Market market)
    {
        this._inventory = market.GetMarketInventory();
        this._market = market;
    }

    public void OnPointerClickFunc(PointerEventData eventData, int index)
    {
        
    }

    public void SwitchItem(MarketItem marketItem)
    {
        SwitchSlot(marketItem);
        SwitchViewModel();
        //_marketStats.UpdateSlot(_currentSlot);
        if (marketItem == MarketItem.Next)
        {
            _arrowRight.Bump();
        }
        else if (marketItem == MarketItem.Previous)
        {
            _arrowLeft.Bump();
        }
        CheckPrice();
        lastMove = marketItem;
    }

    private void SwitchViewModel()
    {
        _lastViewItemRotation = _curMarketModel.transform.GetChild(0).rotation;
        Destroy(_curMarketModel);
        _curMarketModel = Instantiate(_currentSlot.Item.MarketItem, _marketModelPos.position, Camera.main.transform.rotation, _marketCamera.transform);
        _curMarketModel.transform.localScale = _curMarketModel.transform.localScale * _viewItemScaleFactor;
        _curMarketModel.transform.rotation = _lastViewItemRotation;
    }

    private void SwitchSlot(MarketItem marketItem)
    {
        uiSlots[_index].SetActive(false);
        _index = ((_index + (int)marketItem) % _inventory.MaxLength + _inventory.MaxLength) % _inventory.MaxLength;
        _currentSlot = _inventory.Get(_index);
        uiSlots[_index].SetActive(true);
    }

    private void CheckPrice()
    {
        if (InventoryManager.instance.eyes < _currentSlot.Item.Cost || _currentSlot.Item == DatabaseManager.instance.GetEmptyItem())
        {
            _buyButton.interactable = false;
        }
        else
        {
            _buyButton.interactable = true;
        }
    }

    public void OnBuyItem()
    {
        if (InventoryManager.instance.eyes >= _currentSlot.Item.Cost)
        {
            if (_currentSlot.Item != DatabaseManager.instance.GetEmptyItem())
            {
                uiSlots[_index].ClearSlot();
                _market.BuyItem(_index);
                SwitchItem(MarketItem.Current);
            }
        }
    }

    public void OnClose()
    {
    }
}
