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
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool shoot;
		public bool morph;
		public bool equipWeapon1;
		public bool equipWeapon2;
		public bool equipWeapon3;
		public bool reload;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
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
		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
		public void OnMorph(InputValue value)
		{
			MorphInput(value.isPressed);
		}
		public void OnEquipWeapon1(InputValue value)
		{
			EquipWeapon1Input(value.isPressed);
		}
        public void OnEquipWeapon2(InputValue value)
        {
            EquipWeapon2Input(value.isPressed);
        }
        public void OnEquipWeapon3(InputValue value)
        {
            EquipWeapon3Input(value.isPressed);
        }
		public void OnReload(InputValue value) 
		{
			ReloadInput(value.isPressed);
		}
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
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

		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}
		public void MorphInput(bool newMorphState)
		{
			morph = newMorphState;
		}
		public void EquipWeapon1Input(bool newEquipState)
		{ 
			equipWeapon1 = newEquipState;
		}
        public void EquipWeapon2Input(bool newEquipState)
        {
            equipWeapon2 = newEquipState;
        }
        public void EquipWeapon3Input(bool newEquipState)
        {
            equipWeapon3 = newEquipState;
        }

		public void ReloadInput(bool newReloadState)
		{
			reload = newReloadState;
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