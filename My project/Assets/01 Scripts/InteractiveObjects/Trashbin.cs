using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trashbin : InteractiveObject
{
	private void Reset()
	{ 
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.up, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.right, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up + Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up + Vector2.right, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down + Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down + Vector2.right, rayDist = 1f, layer = LayerName.Player},
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
				if (!player || player.carriedItem == null)
					break;
				Destroy(player.carriedItem.gameObject);
				player.carriedItem = null;
			}
		}
	}
}
