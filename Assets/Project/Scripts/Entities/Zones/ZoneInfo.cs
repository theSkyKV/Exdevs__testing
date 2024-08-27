using System.Collections.Generic;
using Project.Entities.Characters;
using UnityEngine;

namespace Project.Entities.Zones
{
	[CreateAssetMenu(fileName = "New Zone Info", menuName = "Zone Info")]
	public class ZoneInfo : ScriptableObject
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private List<BaseCharacter> _enemies;

		[SerializeField]
		private List<int> _weights;

		public string Name => _name;
		public IReadOnlyList<BaseCharacter> Enemies => _enemies;
		public IReadOnlyList<int> Weights => _weights;
	}
}