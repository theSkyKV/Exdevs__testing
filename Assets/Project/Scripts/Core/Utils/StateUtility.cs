using Project.Core.FSM.States;

namespace Project.Core.Utils
{
	public static class StateUtility
	{
		public static string GetName(StateName name)
		{
			return name switch
			{
				StateName.Idle => "Idle",
				StateName.AttackCharging => "Attack Charging",
				StateName.Attack => "Attack",
				StateName.SwitchWeapon => "Switch Weapon",
				_ => ""
			};
		}
	}
}