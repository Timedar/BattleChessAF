using System.Linq;
using UnityEngine;

namespace AFSInterview
{
	public class UnitController : UnitBase
	{
		[SerializeField] private UnitParameters unitParameters = null;

		private float turnSinceLastAttack = 1;
		private int currentHealth = 0;

		private void Awake()
		{
			currentHealth = unitParameters.HealthPoints;
		}

		public override bool CanAttack()
		{
			var canAttack = turnSinceLastAttack >= unitParameters.AttackInterval;
			turnSinceLastAttack += canAttack ? -unitParameters.AttackInterval : 1;
			return canAttack;
		}

		public override void PerformAttack(UnitBase enemyUnitBase, UnitAttributes enemyAttributes)
		{
			var hasSpecialAttack =
				unitParameters.SpecialAttack.FirstOrDefault(x => x.aggainsAttribiute == enemyAttributes);

			var damage = hasSpecialAttack.aggainsAttribiute == UnitAttributes.None
				? unitParameters.AttackDamage
				: hasSpecialAttack.attackDamage;

			enemyUnitBase.ReceiveDamage(damage);
		}

		public override void ReceiveDamage(int damage)
		{
			var calculatedDamage = damage - unitParameters.ArmorPoints;
			currentHealth -= calculatedDamage < 1 ? 1 : calculatedDamage;

			if (currentHealth < 0)
				Destroy(gameObject);
		}
	}
}