using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Player Input State")]
    public bool isAiming;
    public bool isHolding;

    private FirstPersonController firstPersonController;
    [SerializeField] private MarketUI _marketUI;
    [SerializeField] private IUIPanel _currentUIPanel;
    [SerializeField] public Transform dropItemSpawnPos; 
    [SerializeField] public GameObject weaponSpawnPos;
    [SerializeField] public Transform bulletRaycastePos;
    [SerializeField] private Texture2D cursorTexture;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        firstPersonController = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
        Vector2 cursorOffset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.Auto);
    }

    public void NextMarketItem()
    {
        _marketUI.SwitchItem(MarketItem.Next);
    }

    public void PreviousMarketItem()
    {
        _marketUI.SwitchItem(MarketItem.Previous);
    }
}