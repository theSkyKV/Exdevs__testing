using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core.FSM.States;
using Project.Entities.Characters;
using UnityEngine;

namespace Project.Core.FSM
{
	public class CharacterStateMachine
	{
		private readonly BaseCharacter _owner;

		private readonly List<State> _states;
		private readonly LinkedList<State> _statesQueue;
		private State _currentState;

		private IReadOnlyCollection<State> StatesQueue => _statesQueue;

		public event Action<BaseCharacter, AttackCallback> AttackCompleted;
		public event Action<IReadOnlyCollection<State>> StatesQueueUpdated;

		public delegate void AttackCallback();

		public CharacterStateMachine(BaseCharacter owner)
		{
			_owner = owner;

			_states = new List<State>
			{
				new CharacterIdleState(owner),
				new CharacterAttackChargingState(owner),
				new CharacterAttackState(owner),
				new CharacterSwitchWeaponState(owner)
			};

			_statesQueue = new LinkedList<State>();
		}

		public void Execute()
		{
			_currentState?.Execute();
		}

		public void AddToQueue<T>()
		{
			var state = _states.FirstOrDefault(s => s is T);
			if (state == null)
			{
				Debug.LogError($"<CharacterStateMachine::AddToQueue>: State {typeof(T)} does not exist");
				return;
			}

			if (_currentState == null)
			{
				_currentState = state;
				_currentState.Enter();
				_statesQueue.AddFirst(_currentState);
				StatesQueueUpdated?.Invoke(StatesQueue);

				return;
			}

			if (_currentState.Status == StateStatus.Completed)
			{
				_statesQueue.AddLast(state);
				UpdateCurrentState();
			}
			else
			{
				if (_currentState.CanBeInterrupted)
				{
					_statesQueue.AddFirst(state);
					UpdateCurrentState(false);
				}
				else
				{
					_statesQueue.AddLast(state);
				}
			}

			StatesQueueUpdated?.Invoke(StatesQueue);
		}

		public void UpdateCurrentState(bool needRemove = true)
		{
			if (needRemove)
			{
				_statesQueue.RemoveFirst();
				StatesQueueUpdated?.Invoke(StatesQueue);
			}

			_currentState.Exit();

			if (_statesQueue.First == null)
			{
				_currentState = _states[0];
				_statesQueue.AddFirst(_currentState);
				StatesQueueUpdated?.Invoke(StatesQueue);
			}
			else
			{
				_currentState = _statesQueue.First.Value;
			}

			_currentState.Enter();
		}

		public void ResetStates()
		{
			_currentState?.Exit();
			_currentState = null;
			_statesQueue.Clear();

			foreach (var state in _states)
			{
				state.Status = StateStatus.None;
			}

			AddToQueue<CharacterIdleState>();
			StatesQueueUpdated?.Invoke(StatesQueue);
		}

		public void OnAttackCompleted(AttackCallback callback)
		{
			AttackCompleted?.Invoke(_owner, callback);
		}
	}
}