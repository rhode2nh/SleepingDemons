using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Input State")]
    public bool IsAiming;
    public bool IsHolding;
    public bool IsThrowing;
    public float ThrowForce;
    public float HoldOffset = 0.0f;
    public float MaxHoldOffset;
    public float MinHoldOffset;
    public float RotateForce = 1.0f;
    public GameObject Holdable;
    
    [SerializeField] private Texture2D _cursorTexture;

    [SerializeField] public bool IsInBed;
    [SerializeField] public bool InTransitionState;

    public Transform DropItemSpawnPos { get; private set; } 
    public GameObject WeaponSpawnPos { get; private set; }
    public GameObject Player { get; private set; }
    public InputRaycast InputRaycast { get; private set; }
    public bool IsRotating { get; set; }
    public Vector2 RotateVector { get; set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        InputRaycast = FindAnyObjectByType<InputRaycast>();
        // DropItemSpawnPos = Player.GetComponentInChildren<Drop>().transform;
        // WeaponSpawnPos = Player.GetComponentInChildren<GunPos>().gameObject;
        Vector2 cursorOffset = new Vector2(_cursorTexture.width / 2, _cursorTexture.height / 2);
        Cursor.SetCursor(_cursorTexture, cursorOffset, CursorMode.Auto);
    }

    public void MoveHoldPosition(int direction)
    {
        if (!InputRaycast.isHitting) return;
        
        var localPos = InputRaycast.transform.InverseTransformPoint(InputRaycast.hit.transform.position);
        
        if (direction == 1 && localPos.z + HoldOffset < MaxHoldOffset + InputRaycast.maxDistance)
        {
            HoldOffset += 0.1f * direction;
        }
        if (direction == -1 && localPos.z + HoldOffset > MinHoldOffset + InputRaycast.maxDistance)
        {
            HoldOffset += 0.1f * direction;
        }
    }

    private void OnGUI()
    {
        GUI.Box (new Rect (10,10,200,200), new GUIContent(RenderGUIInfo()));
    }

    private string RenderGUIInfo()
    {
        string holdableName = Holdable == null ? "Nothing" : Holdable.name;
        return $"-- Player Manager --\n" +
               $"Is Holding: {IsHolding}\n" +
               $"Holding: {holdableName}\n";
    }
}