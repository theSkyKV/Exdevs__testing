namespace Project.Core.FSM.States
{
	public class GameIdleState : State
	{
		private readonly GameSceneContext _context;
		private readonly GameStateMachine _stateMachine;
		private CharacterStateMachine _playerStateMachine;

		public GameIdleState(GameSceneContext context, GameStateMachine stateMachine)
		{
			_context = context;
			_stateMachine = stateMachine;
			TerminalTime = -1;
		}

		public override void Enter()
		{
			_stateMachine.OnIdleStateEntered();

			_playerStateMachine = _context.PlayerCharacter.StateMachine;
			_playerStateMachine.ResetStates();
		}

		public override void Execute()
		{
			_playerStateMachine.Execute();
		}

		public override void Exit()
		{ }
	}
}