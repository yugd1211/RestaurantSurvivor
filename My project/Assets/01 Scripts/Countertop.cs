using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countertop : InteractiveObject
{
	public int interval;


	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.right, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down + Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down + Vector2.right, rayDist = 1f, layer = LayerName.Player},
			// new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Villain},
			// new InteractionZone {dir = Vector2.right, rayDist = 1f, layer = LayerName.Villain},
			// new InteractionZone {dir = Vector2.down, rayDist = 1f, layer = LayerName.Villain},
			// new InteractionZone {dir = Vector2.down + Vector2.left, rayDist = 1f, layer = LayerName.Villain},
			// new InteractionZone {dir = Vector2.down + Vector2.right, rayDist = 1f, layer = LayerName.Villain},
		};
	}

	private void Update()
	{
		Vector3 forward = transform.TransformDirection(Vector3.up) * 10;
		Debug.DrawRay(transform.position, forward, Color.green);

		List<RaycastHit2D> hits = new();
		interZones.ForEach(interZone =>
			{
				RaycastHit2D hit = FindInteractableAtRay(interZone);
				if (hit)
					hits.Add(hit);
			}
		);

		foreach (RaycastHit2D item in hits)
		{
			switch (item.collider.tag)
			{
				case "Player":
					Debug.Log("Player");
					break;
				case "villain":
					Debug.Log("villain");
					break;
			}
		}
	
		hits.Clear();
	}
}
