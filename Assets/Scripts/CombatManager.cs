using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AFSInterview
{
	public class CombatManager : MonoBehaviour
	{
		[SerializeField] private float timeBetweenTurns = 2f;
		[SerializeField] private ParticleSystem particleSystem;
		[SerializeField] private List<Army> armies;

		private List<UnitBase> unitsOrder = new List<UnitBase>();
		private List<List<UnitBase>> instantiatedArmies = new List<List<UnitBase>>();

		private System.Random random;
		private float nextTurnTime;
		private int currentEnemy = 0;

		private void Awake()
		{
			random = new System.Random(Random.Range(0, int.MaxValue));

			SetupOnFieldArmies(armies);
			ShuffleOrderList(unitsOrder);
		}

		private void SetupOnFieldArmies(List<Army> armyList)
		{
			foreach (var army in armyList)
			{
				var instantiatedUnits = new List<UnitBase>();

				foreach (var unit in army.units)
				{
					var spawnedUnit = SpawnUnit(unit, army.bounds);
					instantiatedUnits.Add(spawnedUnit);
					unitsOrder.Add(spawnedUnit);
				}

				instantiatedArmies.Add(instantiatedUnits);
			}
		}

		private UnitBase SpawnUnit(UnitBase unit, BoxCollider boxCollider)
		{
			var spawnAreaBounds = boxCollider.bounds;
			var position = new Vector3(
				Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
				0f,
				Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
			);

			return Instantiate(unit, position, Quaternion.identity);
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

		private void ProcessTurn(UnitBase unit)
		{
			if (unit.CanAttack())
			{
				AttackRandomEnemy(unit);
				nextTurnTime = Time.time + timeBetweenTurns;
			}
			else
			{
				currentEnemy = (currentEnemy + 1) % unitsOrder.Count;
			}
		}

		private void AttackRandomEnemy(UnitBase unit)
		{
			var enemyIndex = instantiatedArmies[0].Contains(unit) ? 1 : 0;
			var enemyArmy = instantiatedArmies[enemyIndex];
			var randomEnemy = enemyArmy[random.Next(0, enemyArmy.Count)];
			unit.PerformAttack(randomEnemy);

			SetParticleSystem(randomEnemy.transform.position, unit.transform.position);

			Debug.Log($"{unit.gameObject.name} attacks {randomEnemy.gameObject.name}");
		}

		private void SetParticleSystem(Vector3 target, Vector3 origin)
		{
			particleSystem.transform.position = target;
			var shape = particleSystem.shape;
			shape.position = particleSystem.transform.InverseTransformPoint(origin) + Vector3.up;

			var velocity = particleSystem.velocityOverLifetime;
			velocity.radialMultiplier /= timeBetweenTurns;

			particleSystem.startLifetime = timeBetweenTurns;
			particleSystem.Play();
		}

		private void Update()
		{
			if (Time.time >= nextTurnTime)
				ProcessTurn(unitsOrder[currentEnemy]);
		}
	}

	[Serializable]
	public struct Army
	{
		public List<UnitBase> units;
		public BoxCollider bounds;
	}
}