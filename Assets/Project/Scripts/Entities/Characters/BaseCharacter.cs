using System;
using Project.Core.FSM;
using Project.Entities.Attributes;
using UnityEngine;

namespace Project.Entities.Characters
{
	public class BaseCharacter : MonoBehaviour
	{
		[SerializeField]
		private CharacterInfo _info;

		public string Name { get; private set; }
		public CombatAttribute CombatAttribute { get; private set; }
		public CharacterStateMachine StateMachine { get; private set; }
		public BaseCharacter Target { get; set; }
		public bool IsMeleeWeapon { get; set; }

		public event Action<int, int> HealthChanged;
		public event Action<bool> WeaponSwitched;

		public void Construct()
		{
			Name = _info.Name;
			CombatAttribute = new CombatAttribute
			{
				MaxHealth = _info.MaxHealth,
				Power = _info.Power,
				Armor = _info.Armor,
				AttackChargingTime = _info.AttackChargingTime,
				AttackTime = _info.AttackTime
			};

			RestoreHealth();
			IsMeleeWeapon = true;

			StateMachine = new CharacterStateMachine(this);
		}

		public void Initialize()
		{
			OnHealthChanged();
			OnWeaponSwitched();
		}

		public void RestoreHealth()
		{
			CombatAttribute.Health = CombatAttribute.MaxHealth;
			OnHealthChanged();
		}

		public void OnHealthChanged()
		{
			if (CombatAttribute.Health < 0)
			{
				CombatAttribute.Health = 0;
			}

			HealthChanged?.Invoke(CombatAttribute.Health, CombatAttribute.MaxHealth);
		}

		public void OnWeaponSwitched()
		{
			WeaponSwitched?.Invoke(IsMeleeWeapon);
		}
	}
}