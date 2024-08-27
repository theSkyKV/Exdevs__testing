using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Core.Utils
{
	public static class RandomUtility
	{
		public static int GetRandomIndex(List<int> weights)
		{
			var totalWeight = weights.Sum();
			var number = Random.Range(0, totalWeight);
			var sum = 0;
			for (var i = 0; i < weights.Count; i++)
			{
				sum += weights[i];
				if (number < sum)
				{
					return i;
				}
			}

			return 0;
		}
	}
}