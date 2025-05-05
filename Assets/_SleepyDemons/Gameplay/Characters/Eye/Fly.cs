using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField] private GameObject _goToPosition;
    [SerializeField] private float _force;
    [SerializeField] private float _turnSpeed;
    private Rigidbody _rb;

    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        _rb.AddForce((_goToPosition.transform.position - _rb.position) * _force);
        RotateToVelocity(_turnSpeed, false);
    }   

    public void RotateToVelocity(float turnSpeed, bool ignoreY)
    {
        Vector3 dir;
        dir = _rb.linearVelocity;

        if (dir.magnitude > 0.1)
        {
            Quaternion dirQ = Quaternion.LookRotation(dir);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
            _rb.MoveRotation(slerp);
        }
    }
}
