using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InteractionZone
{
	public Vector2 dir;
	public float rayDist;
	public LayerName layer;
}

public enum LayerName
{
	Interactive,
	Player,
	Villain,
	Customer,
}

public abstract class InteractiveObject : MonoBehaviour
{
	public bool isInteractable;
	public List<InteractionZone> interZones;

	protected virtual void Awake()
	{
		isInteractable = true;
	}

	private RaycastHit2D FindInteractableAtRay(InteractionZone interZone)
	{
		return Physics2D.Raycast(transform.position, interZone.dir, interZone.rayDist, LayerMask.GetMask(interZone.layer.ToString()));
	}
	
	protected List<RaycastHit2D> FindInteractableObjects()
	{		
		List<RaycastHit2D> hits = new ();
		interZones.ForEach(interZone =>
			{
				RaycastHit2D hit = FindInteractableAtRay(interZone);
				if (hit)
					hits.Add(hit);
			}
		);
		return hits;
	}
	
	protected Player SearchPlayer()
	{
		List<RaycastHit2D> hits = FindInteractableObjects();
		foreach (RaycastHit2D item in hits)
		{
			if (item.transform.TryGetComponent(out Player player)) 
				return player;
		}
		return null;
	}

	protected void DisplayRay()
	{
		interZones.ForEach(interZone =>
			Debug.DrawRay(transform.position, interZone.dir.normalized * interZone.rayDist, Color.red)
		);
	}
}