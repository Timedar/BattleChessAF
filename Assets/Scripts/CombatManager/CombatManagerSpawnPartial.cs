using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview
{
	public partial class CombatManager
	{
		private void SetupOnFieldArmies(List<Army> armyList)
		{
			foreach (var army in armyList)
			{
				var instantiatedUnits = new List<UnitBase>();

				foreach (var unit in army.Units)
				{
					var spawnedUnit = SpawnUnit(unit, army);
					instantiatedUnits.Add(spawnedUnit);
					unitsOrder.Add(spawnedUnit);
				}

				instantiatedArmies.Add(instantiatedUnits);
			}
		}

		private UnitBase SpawnUnit(UnitBase unit, Army armyProperties)
		{
			var spawnAreaBounds = armyProperties.BoundColider.bounds;
			var position = new Vector3(
				Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
				0f,
				Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
			);

			return Instantiate(unit, position, Quaternion.identity, armyProperties.ArmyContainer);
		}

		private void ShuffleOrderList(List<UnitBase> list)
		{
			int count = list.Count;
			while (count > 1)
			{
				count--;
				int randomIndex = random.Next(count + 1);
				UnitBase temp = list[randomIndex];
				list[randomIndex] = list[count];
				list[count] = temp;
			}
		}
	}
}