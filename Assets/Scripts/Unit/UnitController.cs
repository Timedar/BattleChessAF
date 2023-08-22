using System.Linq;
using UnityEngine;

namespace AFSInterview
{
	public class UnitController : UnitBase
	{
		[SerializeField] private UnitParameters unitParameters = null;
		private int currentHealth;

		private void Awake()
		{
			currentHealth = unitParameters.HealthPoints;
		}

		public override void PerformAttack(UnitBase enemyUnitBase, UnitAttributes enemyAttributes)
		{
			SpecialAttack? hasSpecialAttack =
				unitParameters.SpecialAttack.First(x => x.aggainsAttribiute == enemyAttributes);

			var damage = hasSpecialAttack != null ? hasSpecialAttack.Value.attackDamage : unitParameters.AttackDamage;

			enemyUnitBase.ReceiveDamage(damage);
		}

		public override void ReceiveDamage(int damage)
		{
			var calculatedDamage = damage - unitParameters.ArmorPoints;
			currentHealth -= calculatedDamage < 1 ? 1 : calculatedDamage;
		}
	}
}