using System.Collections.Generic;
using Project.Core.FSM;
using Project.Core.FSM.States;
using Project.Entities.Characters;
using Project.UI;

namespace Project.Core
{
	public class EventBus
	{
		private readonly GameStateMachine _stateMachine;
		private readonly BaseCharacter _playerCharacter;
		private readonly GameUI _ui;

		public EventBus(GameStateMachine stateMachine, BaseCharacter playerCharacter, GameUI ui)
		{
			_stateMachine = stateMachine;
			_playerCharacter = playerCharacter;
			_ui = ui;
		}

		public void Initialize()
		{
			_stateMachine.IdleStateEntered += OnIdleStateEntered;
			_stateMachine.BattleStateEntered += OnBattleStateEntered;

			_playerCharacter.StateMachine.StatesQueueUpdated += OnStatesQueueUpdated;
			_playerCharacter.HealthChanged += OnHealthChanged;
			_playerCharacter.WeaponSwitched += OnWeaponSwitched;

			_ui.StartBattleButtonClicked += OnStartBattleButtonClicked;
			_ui.LeaveBattleButtonClicked += OnLeaveBattleButtonClicked;
			_ui.HealButtonClicked += OnHealButtonClicked;
			_ui.SwitchWeaponButtonClicked += OnSwitchWeaponButtonClicked;
		}

		public void Destruct()
		{
			_stateMachine.IdleStateEntered -= OnIdleStateEntered;
			_stateMachine.BattleStateEntered -= OnBattleStateEntered;

			_playerCharacter.StateMachine.StatesQueueUpdated -= OnStatesQueueUpdated;
			_playerCharacter.HealthChanged -= OnHealthChanged;
			_playerCharacter.WeaponSwitched -= OnWeaponSwitched;

			_ui.StartBattleButtonClicked -= OnStartBattleButtonClicked;
			_ui.LeaveBattleButtonClicked -= OnLeaveBattleButtonClicked;
			_ui.HealButtonClicked -= OnHealButtonClicked;
			_ui.SwitchWeaponButtonClicked -= OnSwitchWeaponButtonClicked;
		}

		private void OnStartBattleButtonClicked()
		{
			_stateMachine.SwitchState<GameBattleState>();
		}

		private void OnLeaveBattleButtonClicked()
		{
			_stateMachine.SwitchState<GameIdleState>();
		}

		private void OnHealButtonClicked()
		{
			_playerCharacter.RestoreHealth();
		}

		private void OnSwitchWeaponButtonClicked()
		{
			_playerCharacter.StateMachine.AddToQueue<CharacterSwitchWeaponState>();
		}

		private void OnIdleStateEntered()
		{
			_ui.ActivateIdleState();
		}

		private void OnBattleStateEntered()
		{
			_ui.ActivateBattleState();
		}

		private void OnStatesQueueUpdated(IReadOnlyCollection<State> statesQueue)
		{
			_ui.UpdateStatesQueue(statesQueue);
		}

		private void OnHealthChanged(int current, int max)
		{
			_ui.UpdateHealth(current, max);
		}

		private void OnWeaponSwitched(bool isMeleeWeapon)
		{
			_ui.UpdateWeapon(isMeleeWeapon);
		}
	}
}