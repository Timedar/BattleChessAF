using System.Linq;
using UnityEngine;

namespace AFSInterview
{
	public class UnitController : MonoBehaviour, IUnit
	{
		[SerializeField] private UnitParameters unitParameters = null;

		private int currentHealth;

		private void Awake()
		{
			currentHealth = unitParameters.HealthPoints;
		}

		public void PerformAttack(IUnit enemyUnit, UnitAttributes enemyAttributes)
		{
			SpecialAttack? hasSpecialAttack =
				unitParameters.SpecialAttack.First(x => x.aggainsAttribiute == enemyAttributes);

			var damage = hasSpecialAttack != null ? hasSpecialAttack.Value.attackDamage : unitParameters.AttackDamage;

			enemyUnit.ReceiveDamage(damage);
		}

		public void ReceiveDamage(int damage)
		{
			var calculatedDamage = damage - unitParameters.ArmorPoints;
			currentHealth -= calculatedDamage < 1 ? 1 : calculatedDamage;
		}
	}
}