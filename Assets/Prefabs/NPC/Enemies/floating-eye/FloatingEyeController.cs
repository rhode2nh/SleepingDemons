using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEyeController : MonoBehaviour
{
    [SerializeField] private GameObject waypoint;
    [SerializeField] private float margin;
    [SerializeField] private float _speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float torqueStrength;
    [SerializeField] private float maxTorque;
    [SerializeField] private float brakingStrength;
    [SerializeField] private float brakingDistance;

    [SerializeField] private float proportionalGain;
    [SerializeField] private float integralGain;

    [SerializeField] private float minRot = 0.0f;
    [SerializeField] private float maxRot = 1.0f;

    [SerializeField, Range(0, 1)] private float maxSway = 1.0f;

    [field: SerializeField] public float CurrentSpeed { get; private set; }
    [field: SerializeField] public float CurrentRotationSpeed { get; private set; }

    private float positionError;
    private float intergrationStored;

    private Rigidbody _rb;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("sway", Math.Clamp(CurrentSpeed, 0.0f, maxSway));
        _animator.SetFloat("speed", CurrentSpeed);

        var rotX = (Math.Clamp(_rb.angularVelocity.x, 0, 1) - minRot) / (maxRot - minRot);
        var rotY = (Math.Clamp(_rb.angularVelocity.y, 0, 1) - minRot) / (maxRot - minRot);
        _animator.SetFloat("rotX", rotX);
        _animator.SetFloat("rotY", rotY);
    }

    private void FixedUpdate()
    {
        CurrentSpeed = _rb.velocity.magnitude;
        CurrentRotationSpeed = _rb.angularVelocity.magnitude;
        AnotherTest();
    }

    float CalculateP(Vector3 currentPosition, Vector3 targetPosition)
    {
        positionError = (targetPosition - currentPosition).magnitude;
        return proportionalGain * positionError;
    }

    float CalculateI(float error)
    {
        intergrationStored = Mathf.Clamp(intergrationStored + (error * Time.fixedDeltaTime), -1, 1);
        return integralGain * intergrationStored;
    }

    private void LookAtPosition(Vector3 position)
    {
        Vector3 dir;
        dir = _rb.velocity;

        if (dir.magnitude > 0.1)
        {
            Quaternion dirQ = Quaternion.LookRotation(dir);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, dir.magnitude * turnSpeed * Time.deltaTime);
            _rb.MoveRotation(slerp);
        }
    }

    private void RotateTowardsDirection()
    {
        // Get the Rigidbody's velocity
        Vector3 velocity = _rb.velocity;

        // If the Rigidbody is moving
        if (velocity.magnitude > 0.1f)
        {
            // Calculate the desired forward direction (normalized velocity vector)
            Vector3 desiredForward = velocity.normalized;

            // Get the current rotation (as a quaternion)
            Quaternion currentRotation = _rb.rotation;

            // Calculate the target rotation from the desired forward direction
            Quaternion targetRotation = Quaternion.LookRotation(desiredForward);

            // Calculate the difference (delta) between the current and target rotation
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(currentRotation);

            // Get the axis and angle of rotation from the delta rotation
            Vector3 axis;
            float angle;
            deltaRotation.ToAngleAxis(out angle, out axis);

            // Convert the angle to radians
            float angleInRadians = angle * Mathf.Deg2Rad;

            // Apply torque based on the rotation axis and angle, scaled by the strength of the torque
            Vector3 torque = axis * Mathf.Clamp(angleInRadians, 0, maxTorque) * torqueStrength;

            // Apply the torque to the Rigidbody
            _rb.AddTorque(torque, ForceMode.Force);
        }
    }

    private void AnotherTest()
    {
        // 1. Calculate the direction to the waypoint
        Vector3 directionToWaypoint = waypoint.transform.position - _rb.position;
        float distanceToWaypoint = directionToWaypoint.magnitude;

        // 2. Normalize the direction for use in force application
        Vector3 normalizedDirection = directionToWaypoint.normalized;

        // 3. Apply movement force
        if (distanceToWaypoint > brakingDistance)
        {
            // If we're not close to the waypoint, move towards it
            _rb.AddForce(normalizedDirection * _speed, ForceMode.Force);
        }
        else
        {
            // Apply braking force when near the waypoint
            Vector3 brakingForce = -_rb.velocity.normalized * brakingStrength;
            _rb.AddForce(brakingForce, ForceMode.Force);
        }

        // 4. Apply rotation to face the waypoint using AddTorque()
        if (distanceToWaypoint > 0.1f) // Prevent continuous rotation when close to the waypoint
        {
            // Determine the direction we need to face (toward the waypoint)
            Quaternion targetRotation = Quaternion.LookRotation(normalizedDirection);

            // Get the difference between the current rotation and the desired rotation
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(_rb.rotation);

            // Extract axis and angle from the delta rotation
            Vector3 axis;
            float angle;
            deltaRotation.ToAngleAxis(out angle, out axis);

            // Apply torque to rotate towards the target
            if (angle > 0.1f) // Only apply torque if there's a significant angle to rotate
            {
                Vector3 torque = axis * angle * torqueStrength;
                _rb.AddTorque(torque, ForceMode.Acceleration);
            }
        }
        _rb.AddTorque(-_rb.angularVelocity);
    }
}