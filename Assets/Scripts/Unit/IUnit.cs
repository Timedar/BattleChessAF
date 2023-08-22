namespace AFSInterview
{
	public interface IUnit
	{
		void PerformAttack(IUnit enemyUnit, UnitAttributes enemyAttributes);
		void ReceiveDamage(int damage);
	}
}