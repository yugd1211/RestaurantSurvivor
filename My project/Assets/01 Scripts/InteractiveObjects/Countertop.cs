using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Countertop : InteractiveObject, Creatable
{ 
	[SerializeField]
	private Food foodPrefab; 
	private Food _food = null;
	public float createInterval;
	
	public int maxFood;
	private int _currentFoodCount;

	private Coroutine _createFoodCoroutine;

	protected override void Awake()
	{
		base.Awake();
		maxFood = 6;
		_currentFoodCount = 0;
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

	public void Upgrade()
	{
		createInterval /= 2;
		maxFood = 10;
		if (_food)
			_food.maxCount = maxFood;
	}
	
	private void Update()
	{
		DisplayRay();
		if (isInteractable == false || _currentFoodCount <= 0 || _food == null) 
			return;

		Player player = SearchPlayer();
		if (player != null)
			GiveFood(player);
	}

	private Player SearchPlayer()
	{
		List<RaycastHit2D> hits = GetInteracObjsInRayPath();
		foreach (RaycastHit2D item in hits)
		{
			if (item.transform.TryGetComponent(out Player player)) 
				return player;
		}
		return null;
	}
	
	private void GiveFood(Player player)
	{
		if (player.carriedItem == null)
			player.SetItem(Instantiate(foodPrefab, transform.position, Quaternion.identity, transform));
		Food food = player.carriedItem as Food;
		while (food && _currentFoodCount > 0 && food.maxCount > food.CurrentCount)
		{
			food.Increase();
			DecreaseFoodCount();
		}
	}

	public void IncreaseFoodCount()
	{
		_currentFoodCount++;
	}
	
	public void DecreaseFoodCount()
	{
		if (_createFoodCoroutine == null)
			_createFoodCoroutine = StartCoroutine(CreateFoodCoroutine(createInterval));
		_currentFoodCount--;
		if (_currentFoodCount <= 0)
		{
			_currentFoodCount = 0;
			StopCoroutine(_createFoodCoroutine);
			Destroy(_food.gameObject);
		}
	}
	
	private IEnumerator CreateFoodCoroutine(float interval)
	{
		while (_currentFoodCount < maxFood)
		{
			yield return new WaitForSeconds(interval);
			if (isInteractable)
				Create();
		}
		_createFoodCoroutine = null;
	}
	
	public void Create()
	{
		if (!_food)
		{
			_food = Instantiate(foodPrefab, transform.position, Quaternion.identity, transform);
			_food.maxCount = maxFood;
		}
		IncreaseFoodCount();
	}
}
