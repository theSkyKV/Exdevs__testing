using Project.Entities.Characters;
using UnityEngine;

namespace Project.Core.FSM.States
{
	public class CharacterSwitchWeaponState : State
	{
		private readonly BaseCharacter _actor;

		private const float SwitchWeaponTime = 2.0f;

		public CharacterSwitchWeaponState(BaseCharacter actor)
		{
			_actor = actor;
			Name = StateName.SwitchWeapon;
			CanBeInterrupted = false;
			TerminalTime = SwitchWeaponTime;
		}

		public override void Enter()
		{
			Status = StateStatus.None;
			ResetTime();
		}

		public override void Execute()
		{
			ElapsedTime += Time.deltaTime;
			if (Status == StateStatus.Completed || ElapsedTime < TerminalTime)
			{
				return;
			}

			Switch();
		}

		public override void Exit()
		{
			ResetTime();
		}

		private void Switch()
		{
			Status = StateStatus.Completed;
			_actor.IsMeleeWeapon = !_actor.IsMeleeWeapon;
			_actor.OnWeaponSwitched();
			_actor.StateMachine.UpdateCurrentState();
		}
	}
}