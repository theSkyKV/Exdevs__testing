using Project.Entities.Characters;

namespace Project.Core.FSM.States
{
	public class CharacterIdleState : State
	{
		private readonly BaseCharacter _actor;

		public CharacterIdleState(BaseCharacter actor)
		{
			_actor = actor;
			Name = StateName.Idle;
			TerminalTime = -1;
		}

		public override void Enter()
		{
			Status = StateStatus.Completed;
		}

		public override void Execute()
		{ }

		public override void Exit()
		{ }
	}
}