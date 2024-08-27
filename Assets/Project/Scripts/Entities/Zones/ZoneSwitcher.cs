using Project.Core;
using UnityEngine;

namespace Project.Entities.Zones
{
	public class ZoneSwitcher
	{
		public Zone ActiveZone { get; private set; }

		private readonly GameSceneContext _context;
		private Zone[] _zones;

		public ZoneSwitcher(GameSceneContext context)
		{
			_context = context;
		}

		public void Initialize()
		{
			_zones = _context.FindObjects<Zone>();
			if (_zones == null || _zones.Length == 0)
			{
				Debug.LogError("<ZoneSwitcher::Initialize>: No zones has been found");
				return;
			}

			foreach (var zone in _zones)
			{
				zone.Construct(_context.PlayerCharacter);
				zone.gameObject.SetActive(false);
			}

			ActivateZone(_zones[0]);
		}

		public void ActivateZone(Zone zone)
		{
			if (ActiveZone != null)
			{
				ActiveZone.Deactivate();
				ActiveZone.gameObject.SetActive(false);
			}

			zone.Activate();
			zone.gameObject.SetActive(true);
			ActiveZone = zone;
		}
	}
}