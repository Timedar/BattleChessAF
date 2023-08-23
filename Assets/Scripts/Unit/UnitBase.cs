using System;
using UnityEngine;

namespace AFSInterview
{
	public abstract class UnitBase : MonoBehaviour
	{
		protected int currentHealth = 0;
		public bool IsAlive => currentHealth > 0;
		public abstract UnitAttributes GetUnitAttributes();
		public abstract bool CanAttack();
		public abstract void PerformAttack(UnitBase enemyUnitBase);
		public abstract void ReceiveDamage(int damage);
	}
}