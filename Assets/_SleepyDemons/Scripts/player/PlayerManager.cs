using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Input State")]
    public bool IsAiming;
    public bool IsHolding;
    
    [SerializeField] private Texture2D _cursorTexture;

    public Transform DropItemSpawnPos { get; private set; } 
    public GameObject WeaponSpawnPos { get; private set; }
    public GameObject Player { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        DropItemSpawnPos = Player.GetComponentInChildren<Drop>().transform;
        WeaponSpawnPos = Player.GetComponentInChildren<GunPos>().gameObject;
        Vector2 cursorOffset = new Vector2(_cursorTexture.width / 2, _cursorTexture.height / 2);
        Cursor.SetCursor(_cursorTexture, cursorOffset, CursorMode.Auto);
    }
}