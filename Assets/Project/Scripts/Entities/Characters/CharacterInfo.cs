using UnityEngine;

namespace Project.Entities.Characters
{
	[CreateAssetMenu(fileName = "New Character Info", menuName = "Character Info")]
	public class CharacterInfo : ScriptableObject
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private int _maxHealth;

		[SerializeField]
		private int _power;

		[SerializeField]
		private int _armor;

		[SerializeField]
		private float _attackChargingTime;

		[SerializeField]
		private float _attackTime;

		public string Name => _name;
		public int MaxHealth => _maxHealth;
		public int Power => _power;
		public int Armor => _armor;
		public float AttackChargingTime => _attackChargingTime;
		public float AttackTime => _attackTime;
	}
}