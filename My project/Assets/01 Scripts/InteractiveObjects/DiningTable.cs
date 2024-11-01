using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningTable : InteractiveObject, Creatable
{
	// TODO : trash, curtomer(Villan) 객체 구현시 GameObject -> Trash, Customer로 변경
	public Trash trashPrefab;
	public GameObject customer;
	
	private Carryable _obj = null;
	public int deleteCount = 2;

	private void Start()
	{
		Create();
	}



	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up, rayDist = 1f, layer = LayerName.Player},
		};
	}

	private void Update()
	{
		DisplayRay();
		List<RaycastHit2D> hits = GetInteracObjsInRayPath();
		hits.ForEach
		(item =>
			{
				// 음식 놓는 영역
				if (item.transform.position - transform.position == Vector3.up)
				{
					// TODO : Player -> Customer로 변경
					if (!item.transform.TryGetComponent(out Player player))
						return;
					if (player.carriedItem is not Food playerFood)
						return;
					// Destroy(player.carriedItem.gameObject);
					player.carriedItem = null;
					_obj = playerFood;
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
						player.carriedItem = myTrash;
						myTrash.transform.SetParent(player.transform);
						myTrash.transform.localPosition = Vector3.zero;
						_obj = null;
					}
					else
					{
						print(playerTrash);
						if (playerTrash == null) 
							return;
						for (int i = 0; i < myTrash.CurrentCount; i++)
						{
							if (playerTrash.CurrentCount >= playerTrash.maxCount)
								break;
							playerTrash.Increase();
							myTrash.DeCrease();
						}
						if (myTrash.CurrentCount <= 0)
						{
							Destroy(myTrash.gameObject);
							myTrash = null;
						}
					}
				}
			}
		);
	}
	
	private IEnumerator DeleteFood()
	{
		Food food = _obj as Food;
		if (food == null)
			yield break;
		while (food.CurrentCount > 0)
		{
			yield return new WaitForSeconds(1f);
			for (int i = 0; i < deleteCount; i++)
				food.DeCrease();
		}
		Destroy(food.gameObject);
		_obj = null;
		Create();
	}

	public void Create()
	{
		print("Create");
		if (_obj != null)
			return;
		Trash newObj = Instantiate(trashPrefab, transform.position, Quaternion.identity, transform);
		_obj = newObj;
		newObj.Increase();
	}
}
