using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Trashbin : InteractiveObject
{
	public float deleteTime = 1f;
	private float _currentTime = 0f;
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
		DisplayRay();

		Player player = SearchPlayer();
		if (player != null)
			HandlePlayerItemDeletion(player);
		else
			_currentTime = 0f;
	}

	private void HandlePlayerItemDeletion(Player player)
	{
		_currentTime += Time.deltaTime;
		if (_currentTime >= deleteTime)
		{
			DeletePlayerCarriedItem(player);
			_currentTime = 0f;
		}
	}
	
	private void DeletePlayerCarriedItem(Player player)
	{
		if (!player.carriedItem)
			return;
		Destroy(player.carriedItem.gameObject);
		player.carriedItem = null;
	}
}
