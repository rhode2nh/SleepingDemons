using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public float lean;
		public bool jump;
		public bool sprint;
		public bool throwItem;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private Interact interact;
		private PlayerInput _playerInput;
		private bool _checkChamber;
		[SerializeField] private Gun _gun;
		[SerializeField] [ReadOnly] private List<string> _lastActionMap;
		private bool _checkChamberCoroutineStarted;

		void Start()
		{
			_lastActionMap = new List<string>();
			interact = GetComponentInChildren<Interact>();
			_playerInput = GetComponent<PlayerInput>();
			_lastActionMap.Add(_playerInput.currentActionMap.name);
			_checkChamber = false;
			_checkChamberCoroutineStarted = false;
			Cursor.visible = false;
			Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
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
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
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
			if (!PlayerManager.instance.isHolding)
			{
				if (!InventoryManager.instance.equippedWeapon.IsEmpty)
				{
					InventoryManager.instance.equippable.Aim(value.isPressed);
					if (value.isPressed)
					{
						PlayerManager.instance.isAiming = true;
						_playerInput.SwitchCurrentActionMap("Aim");
					}
					else
					{
						PlayerManager.instance.isAiming = false;
						_playerInput.SwitchCurrentActionMap("Player");
					}
				}
			}
		}

		public void OnShoot(InputValue value)
		{
			// _gun.Attack(value.isPressed);
			InventoryManager.instance.equippable.Attack(value.isPressed);
			if (value.isPressed)
			{
				// _gun.Play("Shoot", -1, 0f);
			}
		}

		IEnumerator CheckChamber()
		{
			yield return new WaitForSeconds(InputManager.instance._checkChamberTimerDuration);
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

		public void OnCheckChamber(InputValue value)
		{
			// TODO: Rework when animations are implemented
			InventoryManager.instance.equippable.SwitchToMag(false);
			InventoryManager.instance.equippable.SwitchToChamber(false);
			InventoryManager.instance.equippable.CheckChamber(value.isPressed);
			_playerInput.SwitchCurrentActionMap("Player");
		}
		
		public void OnCheckMagazine(InputValue value)
		{
			// TODO: Rework when animations are implemented
			InventoryManager.instance.equippable.SwitchToMag(false);
			InventoryManager.instance.equippable.SwitchToChamber(false);
			InventoryManager.instance.equippable.CheckMagazine(value.isPressed);
			_playerInput.SwitchCurrentActionMap("Player");
		}
		
		public void OnCheckAmmo(InputValue value)
		{
			if (_checkChamber)
			{
				StopCoroutine(CheckChamber());
				_checkChamberCoroutineStarted = false;
				InventoryManager.instance.equippable.CheckChamber(true);
				_playerInput.SwitchCurrentActionMap("CheckChamber");
			}
			else
			{
				InventoryManager.instance.equippable.CheckMagazine(true);
				_playerInput.SwitchCurrentActionMap("CheckMagazine");
			}
		}

		public void OnEmptyChamber(InputValue value)
		{
			InventoryManager.instance.equippable.EmptyChamber();
		}
		
		public void OnLoadBullet(InputValue value)
		{
			InventoryManager.instance.equippable.LoadBullet();
		}

		public void OnMagToChamber(InputValue value)
		{
			// TODO: Rework to use events when animations are implemented
			InventoryManager.instance.equippable.SwitchToChamber(true);
			InventoryManager.instance.equippable.SwitchToMag(false);
			// _playerInput.SwitchCurrentActionMap("CheckChamber");
		}

		public void OnChamberToMag(InputValue value)
		{
			// TODO: Rework to use events when animations are implemented
			InventoryManager.instance.equippable.SwitchToMag(true);
			InventoryManager.instance.equippable.SwitchToChamber(false);
			// _playerInput.SwitchCurrentActionMap("CheckMagazine");
		}

		public void OnPause(InputValue value)
		{
			InputManager.instance.OpenPanel(UIManager.instance._pauseUI, "Pause");
		}

		public void OnUnpause(InputValue value)
		{
			InputManager.instance.ClosePanel(UIManager.instance._pauseUI);
		}

		public void OnOpenInventory(InputValue value)
		{
			InputManager.instance.OpenPanel(UIManager.instance._inventoryUI, "Inventory");
		}

		public void OnCloseInventory(InputValue value)
		{
			InputManager.instance.ClosePanel(UIManager.instance._inventoryUI);
		}

		public void OnCloseMarket(InputValue value)
		{
			InputManager.instance.ClosePanel(UIManager.instance._marketUI);
		}

		public void OnNextItem(InputValue value)
		{
			PlayerManager.instance.NextMarketItem();
		}

		public void OnPreviousItem(InputValue value)
		{
			PlayerManager.instance.PreviousMarketItem();
		}

		public void OnFlashlight(InputValue value)
		{
			InputManager.instance.isFlashlightOn = !InputManager.instance.isFlashlightOn;
		}
		
		public void OnThrow(InputValue value)
		{
			throwItem = value.isPressed;
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
			move = newMoveDirection;
		}

		public void LeanInput(float newLeanInput)
		{
			lean = newLeanInput;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact.InteractWith(newInteractState);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}

}