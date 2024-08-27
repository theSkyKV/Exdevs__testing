using Project.Entities.Characters;
using UnityEngine;

namespace Project.Core.FSM.States
{
	public class GameBattleState : State
	{
		private readonly GameSceneContext _context;
		private readonly GameStateMachine _stateMachine;

		private BaseCharacter _playerCharacter;
		private BaseCharacter _enemyCharacter;

		private bool _battleStarted;

		private const float SeekEnemyTime = 1.0f;

		public GameBattleState(GameSceneContext context, GameStateMachine stateMachine)
		{
			_context = context;
			_stateMachine = stateMachine;
			TerminalTime = SeekEnemyTime;
		}

		public override void Enter()
		{
			_stateMachine.OnBattleStateEntered();
			ResetTime();
			_battleStarted = false;

			_playerCharacter = _context.PlayerCharacter;
			_playerCharacter.StateMachine.AttackCompleted += OnPlayerAttackCompleted;
		}

		public override void Execute()
		{
			ElapsedTime += Time.deltaTime;

			_playerCharacter.StateMachine.Execute();
			_enemyCharacter?.StateMachine.Execute();

			if (_battleStarted || ElapsedTime < TerminalTime)
			{
				return;
			}

			GetEnemy();
		}

		public override void Exit()
		{
			_playerCharacter.StateMachine.AttackCompleted -= OnPlayerAttackCompleted;
			DeactivateEnemy();
			ResetTime();
		}

		private void GetEnemy()
		{
			_enemyCharacter = _context.ZoneSwitcher.ActiveZone.GetRandomEnemy();
			_enemyCharacter.StateMachine.AttackCompleted += OnEnemyAttackCompleted;
			_enemyCharacter.Target = _playerCharacter;
			_playerCharacter.Target = _enemyCharacter;
			StartBattle();
		}

		private void StartBattle()
		{
			_battleStarted = true;
			_playerCharacter.StateMachine.AddToQueue<CharacterAttackChargingState>();
			_enemyCharacter.StateMachine.AddToQueue<CharacterAttackChargingState>();
		}

		private void OnPlayerAttackCompleted(BaseCharacter actor, CharacterStateMachine.AttackCallback callback)
		{
			DealDamage(actor, actor.Target);

			if (_enemyCharacter.CombatAttribute.Health <= 0)
			{
				DeactivateEnemy();
				ResetTime();
				_battleStarted = false;
				_playerCharacter.StateMachine.ResetStates();

				return;
			}

			callback();
		}

		private void OnEnemyAttackCompleted(BaseCharacter actor, CharacterStateMachine.AttackCallback callback)
		{
			DealDamage(actor, actor.Target);

			if (_playerCharacter.CombatAttribute.Health <= 0)
			{
				DeactivateEnemy();
				_stateMachine.SwitchState<GameIdleState>();

				return;
			}

			callback();
		}

		private void DealDamage(BaseCharacter dealer, BaseCharacter receiver)
		{
			var damage = dealer.CombatAttribute.Power - receiver.Target.CombatAttribute.Armor;
			if (damage <= 0)
			{
				return;
			}

			receiver.CombatAttribute.Health -= damage;
			receiver.OnHealthChanged();
		}

		private void DeactivateEnemy()
		{
			if (_enemyCharacter == null)
			{
				return;
			}

			_playerCharacter.Target = null;
			_enemyCharacter.StateMachine.AttackCompleted -= OnEnemyAttackCompleted;
			_context.ZoneSwitcher.ActiveZone.DeactivateEnemy(_enemyCharacter);
			_enemyCharacter = null;
		}
	}
}