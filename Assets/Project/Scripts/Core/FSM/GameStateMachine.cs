using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core.FSM.States;
using UnityEngine;

namespace Project.Core.FSM
{
	public class GameStateMachine
	{
		private readonly GameSceneContext _context;

		private readonly List<State> _states;
		private State _currentState;

		public event Action IdleStateEntered;
		public event Action BattleStateEntered;

		public GameStateMachine(GameSceneContext context)
		{
			_context = context;

			_states = new List<State>
			{
				new GameIdleState(context, this),
				new GameBattleState(context, this)
			};
		}

		public void Run()
		{
			SwitchState<GameIdleState>();
		}

		public void Execute()
		{
			_currentState.Execute();
		}

		public void SwitchState<T>()
		{
			var state = _states.FirstOrDefault(s => s is T);
			if (state == null)
			{
				Debug.LogError($"<GameStateMachine::SwitchState>: State {typeof(T)} does not exist");
				return;
			}

			_currentState?.Exit();
			_currentState = state;
			_currentState.Enter();
		}

		public void OnIdleStateEntered()
		{
			IdleStateEntered?.Invoke();
		}

		public void OnBattleStateEntered()
		{
			BattleStateEntered?.Invoke();
		}
	}
}