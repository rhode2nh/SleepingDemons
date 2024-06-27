using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private DoorOpenCloseAudio _doorOpenCloseAudio;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private PhysicsDoor _physicsDoor;
    [SerializeField] private float _minCloseAngle;
    [SerializeField] private float _maxCloseAngle;
    [SerializeField] private float _minOpenAngle;
    [SerializeField] private float _maxOpenAngle;

    private JointLimits limits;

    // Start is called before the first frame update
    void Start()
    {
        limits = _hingeJoint.limits;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_doorOpenCloseAudio.IsOpen() && !_physicsDoor.IsHolding()) {
            // _rb.velocity = Vector3.zero;
            // _hingeJoint.limits.min = _minCloseAngle;
            // limits.min = _minCloseAngle;
            // limits.max = _maxCloseAngle;
            // _hingeJoint.limits = limits;
        } else if (!_doorOpenCloseAudio.IsOpen() && _physicsDoor.IsHolding()) {
            limits.min = _minOpenAngle;
            limits.max = _maxOpenAngle;
            _hingeJoint.limits = limits;
        }
    }
}
