using Project.Entities.Characters;
using UnityEngine;

namespace Project.Core.FSM.States
{
	public class CharacterAttackChargingState : State
	{
		private readonly BaseCharacter _actor;

		public CharacterAttackChargingState(BaseCharacter actor)
		{
			_actor = actor;
			Name = StateName.AttackCharging;
			CanBeInterrupted = true;
			TerminalTime = _actor.CombatAttribute.AttackChargingTime;
		}

		public override void Enter()
		{
			if (Status == StateStatus.Interrupted)
			{
				return;
			}

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

			Charge();
		}

		public override void Exit()
		{
			if (Status == StateStatus.Completed)
			{
				ResetTime();
				return;
			}

			Status = StateStatus.Interrupted;
		}

		private void Charge()
		{
			Status = StateStatus.Completed;
			_actor.StateMachine.AddToQueue<CharacterAttackState>();
		}
	}
}