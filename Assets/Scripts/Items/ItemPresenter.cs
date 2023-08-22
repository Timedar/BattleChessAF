namespace AFSInterview.Items
{
	using UnityEngine;

	public class ItemPresenter : MonoBehaviour, IItemHolder
	{
		[SerializeField] protected Item item;
        
		public Item GetItem(bool disposeHolder)
		{
			if (disposeHolder)
				Destroy(gameObject);
			
			return item;
		}
	}
}