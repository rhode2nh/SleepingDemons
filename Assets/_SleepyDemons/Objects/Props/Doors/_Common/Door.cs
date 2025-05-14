using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class Door : Interactable, IObservable
{
    private enum HingePosition
    {
        Center,
        Up,
        Down,
        Left,
        Right,
    }

    private enum HingeZPosition
    {
        Center,
        Front,
        Back,
    }
    
    [Header("Observable Settings")]
    [field: SerializeField, Range(0, 100)] public float RollChance { get; private set; }
    
    [Header("Parameters")]
    [SerializeField] private float _strength = 4.0f;
    [SerializeField] private float _drag = 0.5f;
    [SerializeField] private float _maxVelocity = 1.0f;
    [SerializeField] private float _boxColliderScalar = 1.0f;

    [Header("Hinge Settings")]
    [SerializeField] private HingePosition _hingePosition = HingePosition.Right;
    [SerializeField] private HingeZPosition _hingeZPosition = HingeZPosition.Center;
    [SerializeField] private float _minAngle = -90f;
    [SerializeField] private float _maxAngle = 0f;
    [SerializeField] private Collider _ignoreCollider;
    
    private Rigidbody _rb;
    private HingeJoint _hinge;
    private BoxCollider _boxCollider;
    private LockController _lockController;
    
    private Vector3 _axis;
    private bool _isHolding;
    private int _axisForceSign = 1;
    
    private Vector3 _initialReferencePoint;
    private Vector3 _torqueDir;
    private Vector3 _posToHold;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _hinge = GetComponent<HingeJoint>();
        _boxCollider = GetComponent<BoxCollider>();
        _lockController = GetComponentInChildren<LockController>();
        if (_ignoreCollider)
        {
            Physics.IgnoreCollision(_boxCollider, _ignoreCollider);
        }

        _rb.linearDamping = _drag;
        _rb.angularDamping = _drag;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _isHolding = false;
    }

    void Start()
    {
        CalculateHingePosition();
        CalculateBoxCollider();
        InitializeHinge();
    }

    public override void ExecuteInteraction(GameObject other)
    {
        if (!IsLocked())
        {
            Hold(other.GetComponent<Interact>()._inputRaycast.hit.point, PlayerManager.Instance.IsHolding);
        }
    }

    private bool IsLocked()
    {
        if (_lockController != null)
        {
            return _lockController.IsLocked();
        }
        else
        {
            return false;
        }
    }

    public void Hold(Vector3 hitPoint, bool isHolding) {
        _isHolding = isHolding;
        if (_isHolding) {
            StartCoroutine(HoldObject(hitPoint));
        }
    }

    public void Nudge(float strength, bool open)
    {
        int sign = open ? 1 : -1;
        _rb.AddTorque(transform.up * (sign * strength), ForceMode.Impulse);
    }

    IEnumerator HoldObject(Vector3 hitPoint) {
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, hitPoint);
        var initialReferencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
        var referenceDot = Vector3.Dot(transform.forward, initialReferencePoint);
        _rb.useGravity = false;
        PlayerManager.Instance.Holdable = gameObject;
        while (PlayerManager.Instance.IsHolding) {
            var referencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
            var dotProduct = Vector3.Dot(transform.forward, referencePoint) - referenceDot;
            _rb.AddTorque(_torqueDir * (_axisForceSign * _strength * dotProduct));
            _rb.AddTorque(-_rb.angularVelocity);
        
            yield return new WaitForFixedUpdate();
        }
        PlayerManager.Instance.Holdable = null;
        _rb.useGravity = true;
    }

    public float GetNormalizedVelocity() {
        return (_rb.linearVelocity.magnitude - 0.0f) / (_maxVelocity - 0);
    }

    private void InitializeHinge()
    {
        var limits = _hinge.limits;
        switch (_hingePosition)
        {
            case HingePosition.Up:
            case HingePosition.Right:
                limits.min = _maxAngle;
                limits.max = -_minAngle;
                break;
            case HingePosition.Down:
            case HingePosition.Left:
                limits.min = _minAngle;
                limits.max = _maxAngle;
                break;
        }
        _hinge.limits = limits;
        _hinge.useLimits = true;
    }
    
    private void CalculateHingePosition()
    {
        var center = _boxCollider.center;
        var xHingePos = center.x;
        var yHingePos = center.y;
        var zHingePos = center.z;
        var xAxis = 0;
        var yAxis = 0;
        var zAxis = 0;
        switch (_hingePosition)
        {
            case HingePosition.Center:
                break;
            case HingePosition.Up:
                xAxis = 1;
                _axisForceSign = -1;
                yHingePos += _boxCollider.size.y / 2;
                _torqueDir = transform.right;
                break;
            case HingePosition.Down:
                xAxis = 1;
                yHingePos += -_boxCollider.size.y / 2;
                _torqueDir = transform.right;
                break;
            case HingePosition.Left:
                yAxis = 1;
                _axisForceSign = -1;
                xHingePos += -_boxCollider.size.x / 2;
                _torqueDir = transform.up;
                break;
            case HingePosition.Right:
                yAxis = 1;
                xHingePos += _boxCollider.size.x / 2;
                _torqueDir = transform.up;
                break;
        }

        switch (_hingeZPosition)
        {
            case HingeZPosition.Center:
                break;
            case HingeZPosition.Front:
                zHingePos += _boxCollider.size.z / 2;
                break;
            case HingeZPosition.Back:
                zHingePos += -_boxCollider.size.z / 2;
                break;
        }
        _hinge.anchor = new Vector3(xHingePos, yHingePos, zHingePos);
        _hinge.axis = new Vector3(xAxis, yAxis, zAxis);
    }

    private void CalculateBoxCollider()
    {
        _boxCollider.size *= _boxColliderScalar;
    }

    public void OnRollSuccess()
    {
        Nudge(0.1f, true);
    }

    // private IEnumerator WaitForConditions()
    // {
    //     yield return WaitUntil(() => IsWithinRange())
    // }

    public bool IsWithinRange(GameObject other)
    {
        return Vector3.Distance(other.transform.position, transform.position) <= 2.0f;
    }

    public bool IsLookingAt(GameObject other)
    {
        return true;
    }
}
