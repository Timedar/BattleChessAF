using UnityEngine;
using UnityEngine.Serialization;

namespace AFSInterview.Items
{
	public class ItemConsumable : MonoBehaviour, IItemConsumable
	{
		[SerializeField] private Item itemToAdded;
		[SerializeField] private int moneyToAdded;
		[SerializeField] private ConsumableOption consumableOption;

		public void UseConsumableItem(InventoryController inventoryController)
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