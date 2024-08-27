using Project.Core.FSM;
using Project.Entities.Characters;
using Project.Entities.Zones;
using Project.UI;
using UnityEngine;

namespace Project.Core
{
	public class GameSceneContext : MonoBehaviour
	{
		[SerializeField]
		private BaseCharacter _playerCharacterPrefab;

		[SerializeField]
		private GameUI _ui;

		public BaseCharacter PlayerCharacter { get; private set; }
		public ZoneSwitcher ZoneSwitcher { get; private set; }

		private GameStateMachine _stateMachine;
		private EventBus _eventBus;

		private void Awake()
		{
			_stateMachine = new GameStateMachine(this);

			PlayerCharacter = Instantiate(_playerCharacterPrefab, Vector3.zero, Quaternion.identity);
			PlayerCharacter.Construct();

			ZoneSwitcher = new ZoneSwitcher(this);

			_ui.Construct();

			_eventBus = new EventBus(_stateMachine, PlayerCharacter, _ui);
		}

		private void Start()
		{
			_eventBus.Initialize();
			PlayerCharacter.Initialize();
			ZoneSwitcher.Initialize();
			_ui.Initialize();

			_stateMachine.Run();
		}

		private void Update()
		{
			_stateMachine.Execute();
		}

		private void OnDestroy()
		{
			_ui.Destruct();
			_eventBus.Destruct();
		}

		public T[] FindObjects<T>() where T : MonoBehaviour
		{
			return FindObjectsOfType<T>();
		}
	}
}