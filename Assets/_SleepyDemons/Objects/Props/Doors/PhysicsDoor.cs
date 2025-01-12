using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PhysicsDoor : MonoBehaviour, IHoldable
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private BoxCollider boxCollider;
    
    [Header("Parameters")]
    [SerializeField] private bool isHolding;
    [SerializeField] private bool flipAxis;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isCreeking;
    [SerializeField] private float strength;
    [SerializeField] private AudioSource creek;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float boxColliderScalar = 1.0f;
    
    private Vector3 _axis;

    void Awake() {
        isHolding = false;
        
        CalculatePivot();
        CalculateBoxCollider();
    }

    public void Hold(Vector3 hitPoint, bool isHolding) {
        this.isHolding = isHolding;
        if (this.isHolding) {
            StartCoroutine(HoldObject(hitPoint));
        }
    }

    public void Nudge(float strength)
    {
        if (flipAxis)
        {
            if (flipAxis) {
                rb.AddTorque(-transform.up * strength, ForceMode.Impulse);
            } else {
                rb.AddTorque(transform.up * strength, ForceMode.Impulse);
            }
        }
    }

    IEnumerator HoldObject(Vector3 hitPoint) {
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, hitPoint);
        var intiialReferencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
        var referenceDot = Vector3.Dot(transform.forward, intiialReferencePoint);
        // _rb.angularDrag = 5f;
        while (PlayerManager.instance.isHolding) {
            var referencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
            var dotProduct = Vector3.Dot(transform.forward, referencePoint) - referenceDot;
            if (flipAxis) {
                rb.AddTorque(-transform.up * strength * dotProduct);
            } else {
                rb.AddTorque(transform.up * strength * dotProduct);
            }
            rb.AddTorque(-rb.angularVelocity);

            yield return new WaitForFixedUpdate();
        }
        rb.angularDrag = 0.5f;
    }

    void PlayCreek() {
        if (rb.velocity.magnitude >= 0.01f) {
            isMoving = true;
        } else {
            isMoving = false;
            isCreeking = false;
            creek.Stop();
        }

        if (isMoving && !isCreeking) {
            creek.Play();           
            isCreeking = true;
        }

        if (isMoving) {
            creek.volume = rb.velocity.magnitude;
            creek.pitch = Mathf.Clamp(rb.velocity.magnitude, minPitch, maxPitch);
        }
    }

    public float GetVelocity() {
        return rb.velocity.magnitude;
    }

    public bool IsHolding() {
        return isHolding;
    }

    public float GetNormalizedVelocity() {
        return (rb.velocity.magnitude - 0.0f) / (maxVelocity - 0);
    }

    private void CalculatePivot()
    {
        var center = boxCollider.center;
        hinge.anchor = new Vector3(0.5f, center.y, center.z);
        hinge.axis = new Vector3(0, 1, 0);
    }

    private void CalculateBoxCollider()
    {
        // var size = _boxCollider.size;
        // _boxCollider.size = new Vector3(size.x * boxColliderScalar, size.y * boxColliderScalar, size.z);
        boxCollider.size *= boxColliderScalar;
    }
}
