using UnityEngine;

namespace AFSInterview
{
	public abstract class UnitBase : MonoBehaviour
	{
		public abstract void PerformAttack(UnitBase enemyUnitBase, UnitAttributes enemyAttributes);
		public abstract void ReceiveDamage(int damage);
	}
}