namespace Project.Core.FSM.States
{
	public abstract class State
	{
		public StateName Name { get; set; }
		public StateStatus Status { get; set; }
		public bool CanBeInterrupted { get; set; }
		public float ElapsedTime { get; set; }
		public float TerminalTime { get; set; }

		public abstract void Enter();
		public abstract void Execute();
		public abstract void Exit();

		protected void ResetTime()
		{
			ElapsedTime = 0;
		}
	}
}