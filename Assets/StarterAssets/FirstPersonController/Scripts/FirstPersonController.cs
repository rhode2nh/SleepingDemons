﻿using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float AimSpeed = 2.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float AccelerationRate = 10.0f;
        [Tooltip("Tilt speed and angle of the character")]
		public float Friction = 10.0f;
		public float AirResistance = 1.0f;
        public float TiltSpeed = 7f;
        public float TiltAngle = 5f;
		public float TiltOffset;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		public bool Crouched;
		public bool HitCeiling;
		bool _resetVerticalVelocity;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		public float CeilingOffset = 1.0f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;
		public LayerMask CeilingLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTargetParent;
		public GameObject CinemachineCameraTarget;
		public CinemachineVirtualCamera VirtualCamera;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;
		public PlayerInput PlayerInput;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		public float VerticalVelocity;
		private float _verticalKnockback;
		public float TerminalVelocity = 25.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		public float VelocityFOVScaleFactor;
		public float FOVChangeRate;
		public float FOV;

		private const float Threshold = 0.00001f;

		//--------------------user variables--------------------
		[Tooltip("Show debug info for first person controller function")]
		public bool IsDebug;
        private Quaternion _initialRotation;
        private Vector3 _initialPos;
		private Vector3 _lastLookDirBeforeJump;
		private Vector3 _lastLookDirBeforeShoot;
		public Vector3 InputDirection;
		private Vector3 _lastRightDir;
		private Vector3 _lastForwardDir;
		private bool _captureLastInputDir = true;
		private Vector3 _lerpedInputDir;
		private Vector3 _horizontalKnockbackDir;
		private float _lastMoveSpeed;
		[SerializeField] private bool _smoothCamera;
		[SerializeField] private float _dampening;

		private float _initialHeight = 2.0f;
		private float _currentHeight = 2.0f;
		[SerializeField] private float _crouchHeight;

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera != null) return;
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}

		private void Start()
		{
			VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
			FOV = VirtualCamera.m_Lens.FieldOfView;
            _initialRotation = CinemachineCameraTarget.transform.localRotation;
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_input.TriggerCrouch += Crouch;
			PlayerInput = GetComponent<PlayerInput>();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
			_initialHeight = _controller.height;
		}

		public void ApplyForces(Vector3 rotationalDir, Vector3 forceDir) {

		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			CeilingCheck();
			Move();

			_currentHeight = _controller.height;
		}

		private void LateUpdate()
		{
			if (_smoothCamera)
			{
				SmoothCameraRotation();
			}
			else
			{
				CameraRotation();
			}
		}

		private void Crouch()
		{
			float newYPos;
			if (_input.Crouched)
			{
				_controller.height = _crouchHeight;
				newYPos = transform.position.y - ((_initialHeight - _crouchHeight) / 2.0f);
			}
			else
			{
				_controller.height = _initialHeight;
				newYPos = transform.position.y + ((_initialHeight - _crouchHeight) / 2.0f);
			}
			_controller.enabled = false;
			transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
			_controller.enabled = true;
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - (_controller.height / 2.0f) - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CeilingCheck() {
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + (_controller.height / 2.0f) + CeilingOffset, transform.position.z);
			HitCeiling = Physics.CheckSphere(spherePosition, GroundedRadius, CeilingLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.Look.sqrMagnitude >= Threshold)
			{
				_cinemachineTargetPitch += _input.Look.y * RotationSpeed;
				_rotationVelocity = _input.Look.x * RotationSpeed;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTargetParent.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
				
				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private float _targetYRotation;
		private void SmoothCameraRotation()
		{
			// if there is an input
			// if (_input.look.sqrMagnitude >= _threshold)
			// {
				_cinemachineTargetPitch += _input.Look.y * RotationSpeed;
				_rotationVelocity = _input.Look.x * RotationSpeed;
				_targetYRotation += _rotationVelocity;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				Quaternion goal = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);
				CinemachineCameraTargetParent.transform.localRotation = Quaternion.Slerp( CinemachineCameraTargetParent.transform.localRotation, goal, _dampening * Time.deltaTime);
				// CinemachineCameraTargetParent.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
				
				// rotate the player left and right
				// transform.Rotate(Vector3.up * _rotationVelocity);
				Quaternion currentYRotation = transform.rotation;
				Quaternion targetYRotationRot = Quaternion.Euler(0f, _targetYRotation, 0f);
				transform.rotation = Quaternion.Slerp(currentYRotation, targetYRotationRot, _dampening * Time.deltaTime);
			// }
		}

		private void Move()
		{
			InputDirection = MoveGround();
			_horizontalKnockbackDir = -MoveKnockback() * Time.deltaTime;
			// move the player
			_controller.Move(Vector3.ClampMagnitude(InputDirection, 1f) * (_speed * Time.deltaTime) + new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime + _horizontalKnockbackDir);
            float maxSpeed = 100.0f;
            float normalizedSpeed = _controller.velocity.magnitude / maxSpeed;
			// virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, fov + fov * Mathf.Pow(normalizedSpeed, 2) * velocityFOVScaleFactor, Time.deltaTime * fovChangeRate), 0.0f, 120.0f);

            Quaternion _targetRotation;
			Vector3 _targetPos;
            if (_input.Lean == -1) {
                _targetRotation = Quaternion.Euler(0, 0, TiltAngle) * _initialRotation;
				_targetPos = new Vector3(_initialPos.x - TiltOffset, _initialPos.y, _initialPos.z);
            } else if (_input.Lean == 1) {
                _targetRotation = Quaternion.Euler(0, 0, -TiltAngle) * _initialRotation;
				_targetPos = new Vector3(_initialPos.x + TiltOffset, _initialPos.y, _initialPos.z);
            } else {
                _targetRotation = _initialRotation;
				_targetPos = _initialPos;
            }
            CinemachineCameraTarget.transform.localRotation = Quaternion.Slerp(CinemachineCameraTarget.transform.localRotation, _targetRotation, TiltSpeed * Time.deltaTime);
            CinemachineCameraTarget.transform.localPosition = Vector3.Lerp(CinemachineCameraTarget.transform.localPosition, _targetPos, TiltSpeed * Time.deltaTime);
		}

		private Vector3 MoveAir() {
			float targetSpeed = 0.0f;
			float decelType = AirResistance;
			Vector3 dir = new Vector3();

			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * AirResistance);

			dir = _lastLookDirBeforeJump;

			return dir;
		}

		private Vector3 MoveKnockback() {
			float frictionType = Grounded ? Friction : AirResistance;
			_lastLookDirBeforeShoot = Vector3.Lerp(_lastLookDirBeforeShoot, new Vector3(), Time.deltaTime * frictionType);
			return _lastLookDirBeforeShoot;
		}
		
		private Vector3 MoveGround() {
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = Grounded ? (PlayerManager.Instance.IsAiming ? AimSpeed : _input.Sprint ? SprintSpeed : MoveSpeed) : _lastMoveSpeed;
			if (targetSpeed == 0.0f) {
				targetSpeed = MoveSpeed;
			}
			Vector3 inputDir = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;
			float frictionType = Grounded ? Friction : AirResistance;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.Move == Vector2.zero && _captureLastInputDir) {
				_lastRightDir = transform.right;
				_lastForwardDir = transform.forward;
				inputDir = _lastRightDir * inputDir.x + _lastForwardDir * inputDir.z;
				_captureLastInputDir = false;
			} else if (_input.Move != Vector2.zero) {
				inputDir = transform.right * inputDir.x + transform.forward * inputDir.z;
				_captureLastInputDir = true;
			} 

			_lerpedInputDir = Vector3.Lerp(_lerpedInputDir, inputDir, frictionType * Time.deltaTime);

			// a reference to the players current horizontal velocity
			_speed = targetSpeed;
			return _lerpedInputDir;
		}

		private void OnControllerColliderHit(ControllerColliderHit controllerColliderHit) {
			_lerpedInputDir -= controllerColliderHit.normal * Vector3.Dot(_lerpedInputDir, controllerColliderHit.normal);
			_lastLookDirBeforeShoot -= controllerColliderHit.normal * Vector3.Dot(_lastLookDirBeforeShoot, controllerColliderHit.normal);
		}

		private void JumpAndGravity()
		{
			if (Grounded && !HitCeiling)
			{
				_resetVerticalVelocity = false;
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (VerticalVelocity < 0.0f)
				{
					VerticalVelocity = -2f;
				}

				// Jump
				if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					VerticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					_lastLookDirBeforeJump = (transform.right * Vector3.Dot(_controller.velocity, transform.right) + transform.forward * Vector3.Dot(_controller.velocity, transform.forward)).normalized;
					_lastMoveSpeed = _input.Sprint ? SprintSpeed : MoveSpeed;
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.Jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (Mathf.Abs(VerticalVelocity) < TerminalVelocity)
			{
				if (HitCeiling && !_resetVerticalVelocity) {
					VerticalVelocity = 0f;
					_resetVerticalVelocity = true;
				} 
				VerticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			// Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - (_height / 2.0f) - GroundedOffset, transform.position.z);
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - (_currentHeight / 2.0f) - GroundedOffset, transform.position.z), GroundedRadius);
			if (HitCeiling) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + (_currentHeight / 2.0f) + CeilingOffset, transform.position.z), GroundedRadius);
			Gizmos.color = Color.magenta;
			Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), InputDirection * 3);
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), _horizontalKnockbackDir);
		}

		public void SetMouseSense(float mouseSens) {
			RotationSpeed = mouseSens;
		}

		public float GetMouseSense() {
			return RotationSpeed;
		}

		public void ApplyRecoilKnockback(float knockback, Vector3 direction) {
			VerticalVelocity += Vector3.Dot(CinemachineCameraTarget.transform.forward, -transform.up) * knockback;
			_resetVerticalVelocity = false;
			var forward = CinemachineCameraTarget.transform.forward;
			_lastLookDirBeforeShoot += new Vector3(forward.x, 0.0f, forward.z) * knockback;
		}
	}
}
