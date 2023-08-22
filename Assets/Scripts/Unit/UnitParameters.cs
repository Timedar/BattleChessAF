using System;
using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview
{
	[Serializable]
	public class UnitParameters
	{
		[SerializeField] private UnitAttributes unitAttributes = UnitAttributes.None;
		[SerializeField] private int healthPoints = 100;
		[SerializeField] private int armorPoints = 20;
		[SerializeField, Range(0, 2)] private float attackInterval = 1f;
		[SerializeField] private int attackDamage = 20;
		[SerializeField] private List<SpecialAttack> specialAttack;

		public UnitAttributes Attributes => unitAttributes;
		public int HealthPoints => healthPoints;
		public int ArmorPoints => armorPoints;
		public float AttackInterval => attackInterval;
		public int AttackDamage => attackDamage;
		public List<SpecialAttack> SpecialAttack => specialAttack;
	}
}