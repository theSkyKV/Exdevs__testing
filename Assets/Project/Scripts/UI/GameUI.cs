using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project.Core.FSM.States;
using Project.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
	public class GameUI : MonoBehaviour
	{
		[SerializeField]
		private Button _switchWeaponButton;

		[SerializeField]
		private TMP_Text _statesQueueText;

		[SerializeField]
		private TMP_Text _currentStateTime;

		[SerializeField]
		private TMP_Text _weaponDisplay;

		[SerializeField]
		private TMP_Text _healthDisplay;
		
		[SerializeField]
		private Button _startBattleButton;

		[SerializeField]
		private Button _healBattleButton;
		
		[SerializeField]
		private Button _leaveBattleButton;

		public event Action StartBattleButtonClicked;
		public event Action LeaveBattleButtonClicked;
		public event Action HealButtonClicked;
		public event Action SwitchWeaponButtonClicked;
		
		private IReadOnlyCollection<State> _statesQueue;
		private State _currentState;

		private StringBuilder _stringBuilder;

		public void Construct()
		{
			_stringBuilder = new StringBuilder();
		}

		public void Initialize()
		{
			_startBattleButton.onClick.AddListener(OnStartBattleButtonClicked);
			_leaveBattleButton.onClick.AddListener(OnLeaveBattleButtonClicked);
			_healBattleButton.onClick.AddListener(OnHealButtonClicked);
			_switchWeaponButton.onClick.AddListener(OnSwitchWeaponButtonClicked);
			
			ActivateGeneralUI();
		}

		public void Destruct()
		{
			_startBattleButton.onClick.RemoveListener(OnStartBattleButtonClicked);
			_leaveBattleButton.onClick.RemoveListener(OnLeaveBattleButtonClicked);
			_healBattleButton.onClick.RemoveListener(OnHealButtonClicked);
			_switchWeaponButton.onClick.RemoveListener(OnSwitchWeaponButtonClicked);
		}

		private void ActivateGeneralUI()
		{
			_switchWeaponButton.gameObject.SetActive(true);
			_statesQueueText.gameObject.SetActive(true);
			_currentStateTime.gameObject.SetActive(true);
			_weaponDisplay.gameObject.SetActive(true);
			_healthDisplay.gameObject.SetActive(true);
		}

		public void ActivateIdleState()
		{
			_startBattleButton.gameObject.SetActive(true);
			_healBattleButton.gameObject.SetActive(true);
			_leaveBattleButton.gameObject.SetActive(false);
		}

		public void ActivateBattleState()
		{
			_startBattleButton.gameObject.SetActive(false);
			_healBattleButton.gameObject.SetActive(false);
			_leaveBattleButton.gameObject.SetActive(true);
		}

		private void OnStartBattleButtonClicked()
		{
			StartBattleButtonClicked?.Invoke();
		}

		private void OnLeaveBattleButtonClicked()
		{
			LeaveBattleButtonClicked?.Invoke();
		}

		private void OnHealButtonClicked()
		{
			HealButtonClicked?.Invoke();
		}

		private void OnSwitchWeaponButtonClicked()
		{
			SwitchWeaponButtonClicked?.Invoke();
		}

		public void UpdateStatesQueue(IReadOnlyCollection<State> statesQueue)
		{
			_statesQueue = statesQueue;
			_currentState = _statesQueue.FirstOrDefault();
			UpdateStatesQueueText();
		}

		private void UpdateStatesQueueText()
		{
			_stringBuilder.Clear();
			foreach (var state in _statesQueue)
			{
				_stringBuilder.AppendLine(StateUtility.GetName(state.Name));
			}

			_statesQueueText.text = _stringBuilder.ToString();
		}

		private void Update()
		{
			UpdateStatesQueueTime();
		}

		private void UpdateStatesQueueTime()
		{
			if (_currentState == null)
			{
				return;
			}

			var remainingTime = _currentState.TerminalTime - _currentState.ElapsedTime;
			_currentStateTime.text = remainingTime > 0 ? remainingTime.ToString("F1") : " - ";
		}

		public void UpdateHealth(int current, int max)
		{
			_healthDisplay.text = $"{current.ToString()}/{max.ToString()}";
		}

		public void UpdateWeapon(bool isMeleeWeapon)
		{
			_weaponDisplay.text = isMeleeWeapon.ToString();
		}
	}
}