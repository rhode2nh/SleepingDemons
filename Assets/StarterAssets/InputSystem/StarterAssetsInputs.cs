using System;
using System.Collections;
using System.Collections.Generic;
using ParadoxNotion;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using System.Text;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 Move;
		public Vector2 Look;
		public float Lean;
		public bool Jump;
		public bool Sprint;
		public bool Crouched;
		public bool ThrowItem;

		[Header("Movement Settings")]
		public bool AnalogMovement;

		[Header("Mouse Cursor Settings")]
		public bool CursorLocked = true;
		public bool CursorInputForLook = true;

		// private Interact _interact;
		private PlayerInput _playerInput;
		private bool _checkChamber;
		[SerializeField] [ReadOnly] private List<string> _lastActionMap;
		private bool _checkChamberCoroutineStarted;

		public event Action TriggerCrouch;
		public event Action OnGetOutOfBed;

		void Start()
		{
			_lastActionMap = new List<string>();
			_playerInput = GetComponent<PlayerInput>();
			_lastActionMap.Add(_playerInput.currentActionMap.name);
			_checkChamber = false;
			_checkChamberCoroutineStarted = false;
			Cursor.visible = false;
			Cursor.lockState = CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
		}

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLean(InputValue value)
		{
			LeanInput(value.Get<float>());
		}

		public void OnLook(InputValue value)
		{
			if (CursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
			else
			{
				LookInput(new Vector2());
				PlayerManager.Instance.RotateVector = value.Get<Vector2>();
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

		public void OnReload(InputValue value)
		{
			Debug.Log("Reloading");
		}

		public void OnUnload(InputValue value)
		{
			Debug.Log("Unloading");
		}

		public void OnAim(InputValue value)
		{
			if (PlayerManager.Instance.IsHolding && value.isPressed)
			{
				Debug.Log("Throwing");
				PlayerManager.Instance.IsHolding = false;
				PlayerManager.Instance.IsThrowing = true;
				return;
			};
			if (InventoryManager.instance.selectedSlot.IsEmpty) return;
			
			InventoryManager.instance.Equippable.Aim(value.isPressed);
			if (value.isPressed)
			{
				PlayerManager.Instance.IsAiming = true;
				_playerInput.SwitchCurrentActionMap("Aim");
			}
			else
			{
				PlayerManager.Instance.IsAiming = false;
				_playerInput.SwitchCurrentActionMap("Player");
			}
		}

		public void OnShoot(InputValue value)
		{
			// _gun.Attack(value.isPressed);
			InventoryManager.instance.Equippable.Attack(value.isPressed);
			if (value.isPressed)
			{
				// _gun.Play("Shoot", -1, 0f);
			}
		}

		IEnumerator CheckChamber()
		{
			yield return new WaitForSeconds(InputManager.Instance._checkChamberTimerDuration);
			_checkChamber = false;
			_checkChamberCoroutineStarted = false;
		}

		public void OnSetCheckChamber(InputValue value)
		{
			_checkChamber = true;
			if (_checkChamberCoroutineStarted)
			{
				StopCoroutine(CheckChamber());
			}
			StartCoroutine(CheckChamber());
		}

		public void OnMoveHoldable(InputValue value)
		{
			float input = value.Get<float>();
			if (input != 0.0f)
			{
				PlayerManager.Instance.MoveHoldPosition((int)input);
			}
		}

		public void OnRotateHoldable(InputValue value)
		{
			CursorInputForLook = !value.isPressed;
			PlayerManager.Instance.IsRotating = value.isPressed;
		}
		
		public void OnCheckChamber(InputValue value)
		{
			// TODO: Rework when animations are implemented
			InventoryManager.instance.Equippable.SwitchToMag(false);
			InventoryManager.instance.Equippable.SwitchToChamber(false);
			InventoryManager.instance.Equippable.CheckChamber(value.isPressed);
			_playerInput.SwitchCurrentActionMap("Player");
		}
		
		public void OnCheckMagazine(InputValue value)
		{
			// TODO: Rework when animations are implemented
			InventoryManager.instance.Equippable.SwitchToMag(false);
			InventoryManager.instance.Equippable.SwitchToChamber(false);
			InventoryManager.instance.Equippable.CheckMagazine(value.isPressed);
			_playerInput.SwitchCurrentActionMap("Player");
		}
		
		public void OnCheckAmmo(InputValue value)
		{
			if (_checkChamber)
			{
				StopCoroutine(CheckChamber());
				_checkChamberCoroutineStarted = false;
				InventoryManager.instance.Equippable.CheckChamber(true);
				_playerInput.SwitchCurrentActionMap("CheckChamber");
			}
			else
			{
				InventoryManager.instance.Equippable.CheckMagazine(true);
				_playerInput.SwitchCurrentActionMap("CheckMagazine");
			}
		}

		public void OnEmptyChamber(InputValue value)
		{
			InventoryManager.instance.Equippable.EmptyChamber();
		}
		
		public void OnLoadBullet(InputValue value)
		{
			InventoryManager.instance.Equippable.LoadBullet();
		}

		public void OnMagToChamber(InputValue value)
		{
			// TODO: Rework to use events when animations are implemented
			InventoryManager.instance.Equippable.SwitchToChamber(true);
			InventoryManager.instance.Equippable.SwitchToMag(false);
			// _playerInput.SwitchCurrentActionMap("CheckChamber");
		}

		public void OnChamberToMag(InputValue value)
		{
			// TODO: Rework to use events when animations are implemented
			InventoryManager.instance.Equippable.SwitchToMag(true);
			InventoryManager.instance.Equippable.SwitchToChamber(false);
			// _playerInput.SwitchCurrentActionMap("CheckMagazine");
		}

		public void OnPause(InputValue value)
		{
			InputManager.Instance.OpenUI("Pause");
			UIManager.Instance.OpenPauseUI();
		}

		public void OnUnpause(InputValue value)
		{
			InputManager.Instance.CloseUI();
			UIManager.Instance.ClosePauseUI();
		}

		public void OnOpenInventory(InputValue value)
		{
			InputManager.Instance.OpenUI("Inventory");
			UIManager.Instance.OpenInventoryUI();
		}

		public void OnCloseInventory(InputValue value)
		{
			InputManager.Instance.CloseUI();
			UIManager.Instance.CloseInventoryUI();
		}

		public void OnFlashlight(InputValue value)
		{
			InputManager.Instance.IsFlashlightOn = !InputManager.Instance.IsFlashlightOn;
		}

		public void OnCrouch(InputValue newCrouchState)
		{
			Crouched = !Crouched;
			TriggerCrouch?.Invoke();
		}
		
		public void OnThrow(InputValue value)
		{
			ThrowItem = value.isPressed;
		}

		public void OnQuit(InputValue value)
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
				// Application.Quit();
#endif
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		public void LeanInput(float newLeanInput)
		{
			Lean = newLeanInput;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			Look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			Jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			Sprint = newSprintState;
		}

		public void InteractInput(bool newInteractState)
		{
			if (PlayerManager.Instance.IsInBed && newInteractState)
			{
				OnGetOutOfBed?.Invoke();
			}
			else
			{
				PlayerManager.Instance.IsHolding = newInteractState;
				InputManager.Instance.OnInteract?.Invoke(newInteractState);
			}
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(CursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		public void SetInputActive(bool active)
		{
			_playerInput.enabled = active;
		}
	}

}