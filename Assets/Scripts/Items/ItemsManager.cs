﻿using System;

namespace AFSInterview.Items
{
	using TMPro;
	using UnityEngine;

	public class ItemsManager : MonoBehaviour
	{
		[SerializeField] private InventoryController inventoryController;
		[SerializeField] private int itemSellMaxValue;
		[SerializeField] private Transform itemSpawnParent;
		[SerializeField] private GameObject itemPrefab;
		[SerializeField] private BoxCollider itemSpawnArea;
		[SerializeField] private float itemSpawnInterval;
		[SerializeField] private LayerMask layerMask;

		private float nextItemSpawnTime;
		private TextMeshProUGUI moneyText;
		private new Camera camera;

		private void Awake()
		{
			moneyText = FindObjectOfType<TextMeshProUGUI>();
			camera = Camera.main;
		}

		private void Update()
		{
			if (Time.time >= nextItemSpawnTime)
				SpawnNewItem();

			if (Input.GetMouseButtonDown(0))
				TryPickUpItem();

			if (Input.GetKeyDown(KeyCode.Space))
				inventoryController.SellAllItemsUpToValue(itemSellMaxValue);

			moneyText.text = "Money: " + inventoryController.Money;
		}

		private void SpawnNewItem()
		{
			nextItemSpawnTime = Time.time + itemSpawnInterval;

			var spawnAreaBounds = itemSpawnArea.bounds;
			var position = new Vector3(
				Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
				0f,
				Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
			);

			Instantiate(itemPrefab, position, Quaternion.identity, itemSpawnParent);
		}

		private void TryPickUpItem()
		{
			var ray = camera.ScreenPointToRay(Input.mousePosition);

			if (!Physics.Raycast(ray, out var hit, 100f, layerMask) ||
			    !hit.collider.TryGetComponent<IItemHolder>(out var itemHolder))
				return;

			var item = itemHolder.GetItem(true);

			if (item is ItemConsumable consumableItem)
				consumableItem.Use(inventoryController);
			else
				inventoryController.AddItem(item);

			Debug.Log(
				$"Picked up {item.Name} with value of {item.Value} and now have {inventoryController.ItemsCount} items.");
		}
	}
}