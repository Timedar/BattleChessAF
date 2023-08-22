using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AFSInterview
{
	public class CombatManager : MonoBehaviour
	{
		[SerializeField] private float timeBetweenTurns = 2f;
		[SerializeField] private ParticleSystem particleSystem;
		[SerializeField] private List<Army> armies;

		private List<UnitBase> unitsOreder = new List<UnitBase>();
		private List<List<UnitBase>> instantiateArmy = new List<List<UnitBase>>();

		private System.Random random;
		private float nextTurnTime;
		private int currentEnemy = 0;
		private int turn;

		private void Awake()
		{
			random = new System.Random(Random.Range(0, int.MaxValue));

			SetupOnFieldArmies(armies);
			ShuffleOrderList(unitsOreder);
		}

		private void SetupOnFieldArmies(List<Army> army)
		{
			for (int i = 0; i < army.Count; i++)
			{
				instantiateArmy.Add(new List<UnitBase>());

				foreach (var unit in army[i].units)
				{
					var spawnedUnit = SpawnUnit(unit, this.armies[i].bounds);
					instantiateArmy[i].Add(spawnedUnit);
					unitsOreder.Add(spawnedUnit);
				}
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

			return Instantiate(unit, position, quaternion.identity);
		}

		private void ShuffleOrderList(List<UnitBase> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				UnitBase temp = list[i];
				int randomIndex = random.Next(i, list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		private void ProcessTurn(UnitBase unit)
		{
			if (unit.CanAttack())
			{
				AttackRandomEnemy(unit);
				nextTurnTime = Time.time + timeBetweenTurns;
				return;
			}

			turn++;
			currentEnemy++;
			currentEnemy = currentEnemy > unitsOreder.Count - 1 ? 0 : currentEnemy;
		}

		private void AttackRandomEnemy(UnitBase unit)
		{
			var enemyArmy = instantiateArmy[0].Contains(unit) ? instantiateArmy[1] : instantiateArmy[0];
			var randomEnemy = enemyArmy[random.Next(0, enemyArmy.Count)];
			unit.PerformAttack(randomEnemy);

			SetParticleSystem(randomEnemy.transform.position, unit.transform.position);

			Debug.Log($"{unit.gameObject.name} attack on {randomEnemy.gameObject.name}");
		}

		private void SetParticleSystem(Vector3 target, Vector3 origin)
		{
			particleSystem.transform.position = target;
			var shape = particleSystem.shape;
			shape.position = particleSystem.transform.InverseTransformPoint(origin) + Vector3.up;

			var velocity = particleSystem.velocityOverLifetime;
			velocity.radialMultiplier = velocity.radialMultiplier / timeBetweenTurns;

			particleSystem.startLifetime = timeBetweenTurns;
			particleSystem.Play();
		}

		private void Update()
		{
			if (Time.time >= nextTurnTime)
				ProcessTurn(unitsOreder[currentEnemy]);
		}
	}

	[Serializable]
	public struct Army
	{
		public List<UnitBase> units;
		public BoxCollider bounds;
	}
}