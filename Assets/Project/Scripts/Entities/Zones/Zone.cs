using System.Collections.Generic;
using Project.Core.Utils;
using Project.Entities.Characters;
using UnityEngine;

namespace Project.Entities.Zones
{
	public class Zone : MonoBehaviour
	{
		[SerializeField]
		private Transform _playerCharacterSpawnPoint;

		[SerializeField]
		private Transform _enemyCharacterSpawnPoint;

		[SerializeField]
		private ZoneInfo _info;

		private BaseCharacter _playerCharacter;
		private List<BaseCharacter> _enemies;
		private List<int> _weights;

		public void Construct(BaseCharacter playerCharacter)
		{
			_playerCharacter = playerCharacter;

			var count = _info.Enemies.Count;
			_enemies = new List<BaseCharacter>(count);
			_weights = new List<int>(count);
		}

		public void Activate()
		{
			_playerCharacter.transform.position = _playerCharacterSpawnPoint.position;
			ActivateEnemies();
		}

		public void Deactivate()
		{
			foreach (var enemy in _enemies)
			{
				Destroy(enemy.gameObject);
			}

			_enemies.Clear();
			_weights.Clear();
		}

		private void ActivateEnemies()
		{
			var enemies = _info.Enemies;
			var weights = _info.Weights;
			var isCountEquals = enemies.Count == weights.Count;

			for (var i = 0; i < enemies.Count; i++)
			{
				var enemy = Instantiate(enemies[i], _enemyCharacterSpawnPoint);
				enemy.Construct();
				enemy.gameObject.SetActive(false);
				var weight = isCountEquals ? weights[i] : 1;
				_enemies.Add(enemy);
				_weights.Add(weight);
			}
		}

		public void DeactivateEnemy(BaseCharacter enemy)
		{
			enemy.StateMachine.ResetStates();
			enemy.Target = null;
			enemy.RestoreHealth();
			enemy.gameObject.SetActive(false);
		}

		public BaseCharacter GetRandomEnemy()
		{
			var index = RandomUtility.GetRandomIndex(_weights);
			var enemy = _enemies[index];
			enemy.gameObject.SetActive(true);

			return enemy;
		}
	}
}