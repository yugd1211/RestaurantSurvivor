using System.Collections.Generic;
using UnityEngine;

public class CashierTable : InteractiveObject
{
	public Food prefab;
	private Food _food = null;
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
		List<RaycastHit2D> hits = GetInteracObjsInRayPath();
		DisplayRay();

		foreach (RaycastHit2D item in hits)
		{
			if (item.transform.TryGetComponent(out Player player))
			{
				if (!player || player.carriedItem is not Food)
					break;
				if (!_food)
				{
					_food = Instantiate(prefab, transform.position, Quaternion.identity, transform);
					_food.maxCount = 99;
				}
				Food playerFood = player.carriedItem as Food;
				while (playerFood.CurrentCount > 0)
				{
					if (_food.CurrentCount >= _food.maxCount)
						break;
					_food.Increase();
					playerFood.DeCrease();
				}
				if (playerFood.CurrentCount == 0)
				{
					Destroy(player.carriedItem.gameObject);
					player.carriedItem = null;
					

				}
			}
		}
	}
}
