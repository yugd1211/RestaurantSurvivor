using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiningTable : InteractiveObject, Creatable
{
	public Trash trashPrefab;
	public DiningTableInteractable Guest;
	public bool isOccupied;

	private Carryable _obj = null;
	public int deleteCount = 2;

	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up, rayDist = 1f, layer = LayerName.Customer},
		};
	}

	public bool IsAvailable()
	{
		return !isOccupied && !_obj;
	}

	private void Update()
	{
		DisplayRay();
		if (!isInteractable)
			return;

		Player player = SearchPlayer();
		if (player != null)
			CleanTrashFromTable(player);
		
		Customer customer = SearchCustomer();
		if (customer != null)
			PlaceFoodFromCustomer(customer);
	}

	private Customer SearchCustomer()
	{
		List<RaycastHit2D> hits = FindInteractableObjects();
		foreach (RaycastHit2D item in hits)
		{
			if (item.collider.TryGetComponent(out Customer customer))
				return customer;
		}
		return null;
	}

	private void PlaceFoodFromCustomer(Customer customer)
	{
		_obj = customer.food;
		if (_obj == null) return;

		customer.food = null;
		_obj.transform.SetParent(transform);
		_obj.transform.localPosition = Vector3.zero;
		EatFood();
	}

	private void CleanTrashFromTable(Player player)
	{
		if (_obj is not Trash tableTrash || tableTrash == null)
			return;

		Trash playerTrash = player.carriedItem as Trash;
		if (player.carriedItem == null)
			SetTrashToPlayer(player);
		else if (playerTrash != null)
			TransferTrashToPlayer(playerTrash, tableTrash);
	}

	private void SetTrashToPlayer(Player player)
	{
		Trash newObj = Instantiate(trashPrefab, transform.position, Quaternion.identity, transform);
		player.SetItem(newObj);
		newObj.maxCount = player.maxStorage;
	}

	private void TransferTrashToPlayer(Trash playerTrash, Trash tableTrash)
	{
		for (int i = 0; i < tableTrash.CurrentCount; i++)
		{
			if (playerTrash.CurrentCount >= playerTrash.maxCount)
				break;

			playerTrash.Increase();
			tableTrash.Decrease();
		}
	}

	private void EatFood()
	{
		StartCoroutine(DeleteFood());
	}

	private IEnumerator DeleteFood()
	{
		Food food = _obj as Food;
		int foodCount = _obj.CurrentCount;

		yield return DecreaseFoodWithDeleteCount(food);
		_obj = null;
		
		Create();
		IncreaseTrash(foodCount);
		RemoveCustomer();
		AttemptToSpawnVillain();
	}

	private IEnumerator DecreaseFoodWithDeleteCount(Food food)
	{
		while (food.CurrentCount > 0)
		{
			yield return new WaitForSeconds(1f);
			for (int i = 0; i < deleteCount; i++)
			{
				if (food.CurrentCount > 0)
					food.Decrease();
			}
		}
	}

	private void IncreaseTrash(int count)
	{
		for (int i = 0; i < count; i++)
		{
			_obj.Increase();
		}
	}

	private void RemoveCustomer()
	{
		if (Guest is Customer customer)
			customer.Destroy();
	}

	private void AttemptToSpawnVillain()
	{
		if (Random.Range(0, 5) != 0 || !VillainManager.Instance.GetVillain<DiningTableInteractable>(out Villain villain)) 
			return;
		if (villain is not DiningTableVillain tableVillain) 
			return;
		tableVillain.diningTable = this;
		tableVillain.MoveTo();
		Guest = tableVillain;
	}

	public void Create()
	{
		if (_obj != null)
			return;

		Trash newObj = Instantiate(trashPrefab, transform.position, Quaternion.identity, transform);
		_obj = newObj;
	}
}
