using Project.Entities.Characters;
using UnityEngine;

namespace Project.Core.FSM.States
{
	public class CharacterAttackState : State
	{
		private readonly BaseCharacter _actor;

		public CharacterAttackState(BaseCharacter actor)
		{
			_actor = actor;
			Name = StateName.Attack;
			CanBeInterrupted = false;
			TerminalTime = _actor.CombatAttribute.AttackTime;
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

			Attack();
		}

		public override void Exit()
		{
			ResetTime();
		}

		private void Attack()
		{
			Status = StateStatus.Completed;
			_actor.StateMachine.OnAttackCompleted(AttackCallback);
		}

		private void AttackCallback()
		{
			_actor.StateMachine.AddToQueue<CharacterAttackChargingState>();
		}
	}
}