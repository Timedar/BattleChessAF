using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace AFSInterview
{
	public partial class CombatManager : MonoBehaviour
	{
		[SerializeField] private float timeBetweenTurns = 2f;
		[SerializeField] private AttackVisualisation attackVisualisation;
		[SerializeField] private List<Army> armies;

		private List<UnitBase> unitsOrder = new List<UnitBase>();
		private List<List<UnitBase>> instantiatedArmies = new List<List<UnitBase>>();

		private System.Random random;
		private float nextTurnTime;
		private UnitBase currentUnit;
		private bool areAnyArmy => (instantiatedArmies[0].Count > 0) && (instantiatedArmies[1].Count > 0);

		private void Awake()
		{
			random = new System.Random(Random.Range(0, int.MaxValue));

			SetupOnFieldArmies(armies);
			ShuffleOrderList(unitsOrder);

			currentUnit = unitsOrder[0];
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
				SetupNewCurrentUnit();
			}
		}

		private void SetupNewCurrentUnit()
		{
			var newUnitIndex = (unitsOrder.IndexOf(currentUnit) + 1) % unitsOrder.Count;
			currentUnit = unitsOrder[newUnitIndex];
		}

		private void AttackRandomEnemy(UnitBase unit)
		{
			var randomEnemy = GetRandomEnemy(unit);
			unit.PerformAttack(randomEnemy);

			if (!randomEnemy.IsAlive)
				DestoryUnit(randomEnemy);

			attackVisualisation.SetParticleSystem(randomEnemy.transform.position, unit.transform.position,
				timeBetweenTurns);
		}

		private void DestoryUnit(UnitBase unit)
		{
			unitsOrder.Remove(unit);
			var armyIndex = GetArmyIndex(unit);
			instantiatedArmies[armyIndex].Remove(unit);

			Destroy(unit.gameObject, timeBetweenTurns);
		}

		private UnitBase GetRandomEnemy(UnitBase unit)
		{
			var enemyIndex = GetArmyIndex(unit) == 0 ? 1 : 0;
			var enemyArmy = instantiatedArmies[enemyIndex];
			var randomEnemy = enemyArmy[random.Next(0, enemyArmy.Count - 1)];
			return randomEnemy;
		}

		private int GetArmyIndex(UnitBase unit)
		{
			return instantiatedArmies[0].Contains(unit) ? 0 : 1;
		}

		private void Update()
		{
			if (Time.time >= nextTurnTime && areAnyArmy)
				ProcessTurn(currentUnit);
		}
	}

	[Serializable]
	public struct Army
	{
		public List<UnitBase> Units;
		public Transform ArmyContainer;
		[FormerlySerializedAs("Bounds")] public BoxCollider BoundColider;
	}
}