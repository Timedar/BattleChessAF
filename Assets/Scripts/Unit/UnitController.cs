using System.Linq;
using UnityEngine;

namespace AFSInterview
{
	public class UnitController : UnitBase
	{
		[SerializeField] private UnitParameters unitParameters = null;

		private float turnsSinceLastAttack = 1;
		private int currentHealth = 0;

		private void Awake()
		{
			currentHealth = unitParameters.HealthPoints;
		}

		public override UnitAttributes GetUnitAttributes() => unitParameters.Attributes;

		public override bool CanAttack()
		{
			bool canAttack = turnsSinceLastAttack >= unitParameters.AttackInterval;
			turnsSinceLastAttack += canAttack ? -unitParameters.AttackInterval : 1;
			return canAttack;
		}

		public override void PerformAttack(UnitBase enemyUnitBase)
		{
			var specialAttack = unitParameters.SpecialAttack.FirstOrDefault(x =>
				x.aggainsAttribiute == enemyUnitBase.GetUnitAttributes());

			var damage = specialAttack.aggainsAttribiute == UnitAttributes.None
				? unitParameters.AttackDamage
				: specialAttack.attackDamage;

			enemyUnitBase.ReceiveDamage(damage);
		}

		public override void ReceiveDamage(int damage)
		{
			var calculatedDamage = damage - unitParameters.ArmorPoints;
			currentHealth -= Mathf.Max(1, calculatedDamage);

			TryHandleDeath();
		}

		private void TryHandleDeath()
		{
			if (currentHealth <= 0)
				Destroy(gameObject);
		}
	}
}