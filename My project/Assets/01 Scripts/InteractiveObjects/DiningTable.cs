using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiningTable : InteractiveObject, Creatable
{
	public Trash trashPrefab;
	public DiningTableInteractable guest;
	public bool isOccupied;
	
	private Carryable _obj = null;
	public int deleteCount = 2;


	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Customer},
			new InteractionZone {dir = Vector2.up, rayDist = 1f, layer = LayerName.Customer},
		};
	}
	
	public bool IsAvailable()
	{
		if (isOccupied || _obj != null)
			return false;
		return true;
	}

	private void Update()
	{
		DisplayRay();
		if (isInteractable == false)
			return;
		List<RaycastHit2D> hits = FindInteractableObjects();
		hits.ForEach
		(item =>
			{
				// 음식 놓는 영역
				if (item.collider.TryGetComponent(out Customer customer))
				{
					_obj = customer.food;
					if (_obj == null)
						return;
					customer.food = null;
					_obj.transform.SetParent(transform);
					_obj.transform.localPosition = Vector3.zero;
					StartCoroutine(DeleteFood());
				}
				// 쓰레기 치우는 영역
				else
				{
					if (!item.transform.TryGetComponent(out Player player))
						return;
					Trash playerTrash = player.carriedItem as Trash;
					Trash myTrash = _obj as Trash;
					if (_obj == null || myTrash == null)
						return;
					if (player.carriedItem == null)
					{
						print("플레이어에게 쓰레기 전달 null");
						Trash newObj = Instantiate(trashPrefab, transform.position, Quaternion.identity, transform);
						player.SetItem(newObj);
						newObj.maxCount = player.maxStorage;
						// player.carriedItem = myTrash;
						// myTrash.transform.SetParent(player.transform);
						// myTrash.transform.localPosition = Vector3.zero;
						// _obj = null;
					}
					else
					{
						print("플레이어에게 쓰레기 전달 이미있음 1");

						if (playerTrash == null) 
							return;
						print("플레이어에게 쓰레기 전달 이미있음 2");
						for (int i = 0; i < myTrash.CurrentCount; i++)
						{
							print("플레이어에게 쓰레기 전달 이미있음 3");
							print($"before 쓰레기 {playerTrash.CurrentCount}, {playerTrash.maxCount}, {playerTrash.CurrentCount >= playerTrash.maxCount}");;
							if (playerTrash.CurrentCount >= playerTrash.maxCount)
								break;

							playerTrash.Increase();
							myTrash.DeCrease();
							print($"player = {playerTrash.CurrentCount}, myTrash = {myTrash.CurrentCount}");
							print($"after 쓰레기 {playerTrash.CurrentCount}, {playerTrash.maxCount}, {playerTrash.CurrentCount >= playerTrash.maxCount}");;
							print("플레이어에게 쓰레기 전달 이미있음 4");
						}
						print("플레이어에게 쓰레기 전달 이미있음 5");
					}
				}
			}
		);
	}
	
	private IEnumerator DeleteFood()
	{
		Food food = _obj as Food;
		int count = _obj.CurrentCount;
		while (food.CurrentCount > 0)
		{
			yield return new WaitForSeconds(1f);
			for (int i = 0; i < deleteCount; i++)
			{
				if (food.CurrentCount > 0)
					food.DeCrease();
			}
		}
		_obj = null;
		Create();
		for (int i = 0; i < count; i++)
			_obj.Increase();
		((Customer)guest).Destroy();
		if (Random.Range(0, 5) == 0 && VillainManager.Instance.GetVillain<DiningTableInteractable>(out Villain villain))
		{
			isOccupied = true;
			if (villain is DiningTableVillain tableVillain) tableVillain.diningTable = this;
			villain.MoveTo();
			guest = villain as DiningTableInteractable;
		}
	}

	public void Create()
	{
		if (_obj != null)
			return;
		Trash newObj = Instantiate(trashPrefab, transform.position, Quaternion.identity, transform);
		_obj = newObj;
	}
}
