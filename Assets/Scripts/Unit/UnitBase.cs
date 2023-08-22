using UnityEngine;

namespace AFSInterview
{
	public abstract class UnitBase : MonoBehaviour
	{
		public abstract UnitAttributes GetUnitAttributes();
		public abstract bool CanAttack();
		public abstract void PerformAttack(UnitBase enemyUnitBase);
		public abstract void ReceiveDamage(int damage);
	}
}