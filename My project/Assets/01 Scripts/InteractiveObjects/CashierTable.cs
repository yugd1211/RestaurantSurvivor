using System.Collections.Generic;
using UnityEngine;

public class CashierTable : InteractiveObject, Creatable
{
	public Food prefab; 
	public Food food;
	
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
		
		
		Player player = SearchPlayer();
		if (player?.carriedItem is not Food playerFood || playerFood.CurrentCount <= 0)
			return;
		
		if (!food)
			Create();
		
		ReceiveFoodFromPlayer(player);
	}
	
	public void Create()
	{
		food = Instantiate(prefab, transform.position, Quaternion.identity, transform);
		food.maxCount = 99;
	}

	private void ReceiveFoodFromPlayer(Player player)
	{
		if (player?.carriedItem is not Food playerFood)
			return;
		while (playerFood.CurrentCount > 0)
		{
			if (food.CurrentCount >= food.maxCount)
				break;
			food.Increase();
			playerFood.Decrease();
		}
	}
}
