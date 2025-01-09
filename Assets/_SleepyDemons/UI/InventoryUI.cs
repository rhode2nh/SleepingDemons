using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IUIPanel
{
    private List<UISlot> uiSlots;
    [SerializeField] private GameObject itemsParent;

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    void Awake()
    {
        uiSlots = new List<UISlot>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.instance.onUpdateInventoryGUI += UpdateUI;
        for (int i = 0; i < itemsParent.transform.childCount; i++)
        {
            uiSlots.Add(itemsParent.transform.GetChild(i).GetComponent<UISlot>());
            uiSlots[i].Index = i;
            uiSlots[i].OnPointerClickFunc = OnPointerClickFunc;
            uiSlots[i].SetIconActive(false);
        }
        gameObject.SetActive(false);       
    }

    void UpdateUI()
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (InventoryManager.instance.inventory.Get(i).Item != DatabaseManager.instance.GetEmptyItem())
            {
                uiSlots[i].SetSlot(InventoryManager.instance.inventory.Get(i).Item.Icon);
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }
    }

    void OnPointerClickFunc(PointerEventData eventData, int index)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Slot slot = InventoryManager.instance.Remove(index);
            InventoryManager.instance.UpdateInventoryGUI();
            Instantiate(slot.Item.WorldItem, PlayerManager.instance.dropItemSpawnPos.position, Quaternion.identity);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryManager.instance.UseItem(index);
        }
    }
}
