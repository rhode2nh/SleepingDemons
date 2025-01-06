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

    [SerializeField] private IUIPanel _currentUIPanel;
    [SerializeField] public Transform dropItemSpawnPos; 
    [SerializeField] public GameObject weaponSpawnPos;
    [SerializeField] private Texture2D cursorTexture;
    [field: SerializeField] public GameObject player;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Vector2 cursorOffset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.Auto);
    }
}