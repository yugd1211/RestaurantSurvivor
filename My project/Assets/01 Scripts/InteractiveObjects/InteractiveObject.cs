using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

	private void Awake()
	{
		isInteractable = true;
	}
	protected RaycastHit2D FindInteractableAtRay(InteractionZone interZone)
	{
		return Physics2D.Raycast(transform.position, interZone.dir, interZone.rayDist, LayerMask.GetMask(interZone.layer.ToString()));
	}
	
	protected List<RaycastHit2D> GetInteracObjsInRayPath()
	{		
		List<RaycastHit2D> hits = new List<RaycastHit2D>();
		interZones.ForEach(interZone =>
			{
				RaycastHit2D hit = FindInteractableAtRay(interZone);
				if (hit)
					hits.Add(hit);
			}
		);
		return hits;
	}

	protected void DisplayRay()
	{
		interZones.ForEach(interZone =>
			Debug.DrawRay(transform.position, interZone.dir.normalized * interZone.rayDist, Color.red)
		);
	}
}