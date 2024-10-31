using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countertop : InteractiveObject, Creatable
{ 
	public float createInterval;
	public Food foodPrefab;
	public Food foodObject = null;
	public int upgradeMaxFood;
	public int upgradeInterval;
	
	private int _maxFood;
	private int _currentFoodCount;

	private Coroutine _createFoodCoroutine;

	private void Awake()
	{
		_maxFood = 4;
		_currentFoodCount = 0;
	}
	
	public void Upgrade()
	{
		_maxFood += upgradeMaxFood;
		createInterval = upgradeInterval;
	}

	private void Start()
	{	
		_createFoodCoroutine = StartCoroutine(CreateFoodCoroutine(createInterval));
	}

	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.right, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.down + Vector2.right, rayDist = 1f, layer = LayerName.Player},
		};
	}

	private void Update()
	{
		List<RaycastHit2D> hits = new List<RaycastHit2D>();
		interZones.ForEach(interZone =>
			{
				RaycastHit2D hit = FindInteractableAtRay(interZone);
				if (hit)
					hits.Add(hit);
				Debug.DrawRay(transform.position, interZone.dir.normalized * interZone.rayDist, Color.red);
			}
		);

		foreach (RaycastHit2D item in hits)
		{
			switch (item.collider.tag)
			{
				case "Player":
					if (_currentFoodCount <= 0)
						break;
					PlayerMove player = item.transform.GetComponent<PlayerMove>();
					if (player == null || player?.carriedItem != null && player.carriedItem is not Food)
						break;
					if (player.carriedItem == null)
						player.SetItem(Instantiate(foodPrefab, transform.position, Quaternion.identity, transform));
					Food food = player.carriedItem as Food;
					while (food && _currentFoodCount > 0 && food.maxCount > food.CurrentCount)
					{
						food.Increase();
						DecreaseFoodCount();
					}
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
		if (foodObject == null)
			return null;
		Food result = foodObject;
		foodObject = null;
		DecreaseFoodCount();
		return result;
	}
	
	public void IncreaseFoodCount()
	{
		_currentFoodCount++;
	}
	public void DecreaseFoodCount()
	{
		if (_createFoodCoroutine == null)
		{
			_createFoodCoroutine = StartCoroutine(CreateFoodCoroutine(createInterval));
		}
		_currentFoodCount--;
		if (_currentFoodCount <= 0)
		{
			_currentFoodCount = 0;
			Destroy(foodObject.gameObject);
		}
	}
	
	private IEnumerator CreateFoodCoroutine(float interval)
	{
		while (_currentFoodCount < _maxFood)
		{
			yield return new WaitForSeconds(interval);
			Create();
		}
		_createFoodCoroutine = null;
	}
	
	public void Create()
	{
		if (!foodObject)
			foodObject = Instantiate(foodPrefab, transform.position, Quaternion.identity, transform);
		IncreaseFoodCount();
	}
	
}
