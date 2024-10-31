using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Countertop : InteractiveObject, Creatable
{
	public float createInverval;
	public Food foodPrefab;
	public Food foodObject = null;
	public int upgradeMaxFood;
	public int upgradeInterval;
	
	private int _maxFood;
	private int _currentFoodCount;
	public int CurrentFoodCount 
	{
		get => _currentFoodCount;
		private set
		{
			if (value < 0)
				_currentFoodCount = 0;
			else if (value > _maxFood)
				_currentFoodCount = _maxFood;
			else
				_currentFoodCount = value;
			
			if (value == 0)
				foodObject = null;

		} 
	}
	private Coroutine _createFoodCoroutine;

	private void Awake()
	{
		_maxFood = 4;
		CurrentFoodCount = 0;
	}
	
	public void Upgrade()
	{
		_maxFood += upgradeMaxFood;
		createInverval = upgradeInterval;
	}

	private void Start()
	{
		_createFoodCoroutine = StartCoroutine(CreateFoodCoroutine(createInverval));
	}

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
					PlayerMove player = item.transform.GetComponent<PlayerMove>();
					if (player)
					{
						if (player.carriedItem == null)
							player.SetItem(GetFood());
					}
					Debug.Log("Player");
					break;
				case "Villain":
					Debug.Log("Villain");
					break;
			}
		}
	
		hits.Clear();
	}

	public Carryable GetFood()
	{
		Carryable ret = foodObject;
		if (CurrentFoodCount <= 0)
			return null;
		CurrentFoodCount--;
		if (CurrentFoodCount == 0)
			foodObject = null;
		
		return ret;
	}

	public int GetMoreFood()
	{
		if (CurrentFoodCount > 0)
		{
			if (CurrentFoodCount == upgradeMaxFood)
			{
				_createFoodCoroutine = StartCoroutine(CreateFoodCoroutine(createInverval));
			}
			CurrentFoodCount--;
			return 1;
		}
		return 0;
	}
	
	private IEnumerator CreateFoodCoroutine(float interval)
	{
		while (CurrentFoodCount < _maxFood)
		{
			yield return new WaitForSeconds(interval);
			Create();
		}
	}
	public void Create()
	{
		if (!foodObject)
			foodObject = Instantiate(foodPrefab, transform.position, Quaternion.identity, transform);
		CurrentFoodCount++;
	}
}
