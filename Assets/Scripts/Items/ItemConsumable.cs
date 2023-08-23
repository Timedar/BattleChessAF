using UnityEngine;
using UnityEngine.Serialization;

namespace AFSInterview.Items
{
	public class ItemConsumable : Item, IItemConsumable
	{
		private Item itemToAdded;
		private int moneyToAdded;
		private ConsumableOption consumableOption;

		public ItemConsumable(string name, int value, ConsumableOption option, Item itemToAdd, int moneyToAdd) : base(
			name, value)
		{
			consumableOption = option;
			itemToAdded = itemToAdd;
			moneyToAdded = moneyToAdd;
		}

		public void Use(InventoryController inventoryController) => Consume(inventoryController);

		public void Consume(InventoryController inventoryController)
		{
			switch (consumableOption)
			{
				case ConsumableOption.AddItem:
					inventoryController.AddItem(itemToAdded);
					break;
				case ConsumableOption.AddMoney:
					inventoryController.AddMoney(moneyToAdded);
					break;
			}
		}
	}
}