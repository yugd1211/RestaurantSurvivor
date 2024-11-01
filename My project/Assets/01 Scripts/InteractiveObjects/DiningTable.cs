using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DiningTable : InteractiveObject, Creatable
{
	// TODO : trash, curtomer 객체 구현시 GameObject -> Trash, Customer로 변경
	public GameObject trashPrefab;
	public GameObject customer;
	
	private Carryable _obj = null;
	public int deleteCount = 2;
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
				}
				// 쓰레기 치우는 영역
				else
				{
					
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
	}

	public void Create()
	{
		if (_obj != null)
			return;
		// _obj = Instantiate(prefab, transform.position, Quaternion.identity, transform);
	}
}
