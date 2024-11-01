using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SafeBox : InteractiveObject
{
	public int CurrentMoney { get; private set; } = 0;

	private void Reset()
	{
		interZones = new List<InteractionZone>()
		{
			new InteractionZone {dir = Vector2.down, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.right, rayDist = 1f, layer = LayerName.Player},
		};
	}

	public void IncreaseMoney(int amount)
	{
		CurrentMoney += amount;
	}
	
	public void DecreaseMoney(int amount)
	{
		CurrentMoney -= amount;
		if (CurrentMoney < 0)
			CurrentMoney = 0;
	}

	private void Update()
	{
		List<RaycastHit2D> hits = GetInteracObjsInRayPath();
		DisplayRay();
		hits.ForEach(item =>
			{
				if (!item.transform.TryGetComponent(out Player player) || !player || !player.carriedItem)
					return;
				if (player.carriedItem is not Money playerMoney)
					return;
                
				// 돈 상호 작용 영역
				if (item.transform.position - transform.position == Vector3.down)
				{
					IncreaseMoney(playerMoney.CurrentCount);
					Destroy(playerMoney.gameObject);
				}
				// 강화 상호작용 영역
				else
				{
					
				}

			}
		);
	}
}
