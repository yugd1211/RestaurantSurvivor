using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CashierTable : InteractiveObject
{
	public Food prefab; 
	public Food food = null;
	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.right, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up + Vector2.right, rayDist = 1f, layer = LayerName.Player},
		};
	}
	
	private void Update()
	{		
		DisplayRay();

		if (isInteractable == false)
			return;
		List<RaycastHit2D> hits = GetInteracObjsInRayPath();
		foreach (RaycastHit2D item in hits)
		{
			if (item.transform.TryGetComponent(out Player player))
			{
				if (!player || player.carriedItem is not Food)
					break;
				if (!food)
				{
					food = Instantiate(prefab, transform.position, Quaternion.identity, transform);
					food.maxCount = 99;
				}
				Food playerFood = player.carriedItem as Food;
				while (playerFood.CurrentCount > 0)
				{
					if (food.CurrentCount >= food.maxCount)
						break;
					food.Increase();
					playerFood.DeCrease();
				}
			}
		}
	}
}
