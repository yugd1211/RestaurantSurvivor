using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countertop : InteractiveObject, Creatable
{ 
	[SerializeField]
	private Food foodPrefab; 
	private Food _food;
	
	
	public float createInterval;
	public int maxFood;
	
	private Coroutine _createFoodCoroutine;
	private int _currentFoodCount;
	
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
	
	private void Update()
	{
		DisplayRay();

		if (!isInteractable) 
			return;

		Player player = SearchPlayer();
		if (player == null)
			return;
		
		HandlePlayerInteraction(player);
	}
	
	private void HandlePlayerInteraction(Player player)
	{
		if (_food == null)
			return;
		
		if (player.carriedItem == null)
			CreateFoodToPlayer(player);
		else if (player.carriedItem is Food playerFood)
			TransferFoodToPlayer(playerFood);
	}
	
	
		
	public void Create()
	{

		_food = Instantiate(foodPrefab, transform.position, Quaternion.identity, transform);
		_food.maxCount = maxFood;
	}

	public void Upgrade()
	{
		createInterval /= 2;
		maxFood = 10;
		if (_food)
			_food.maxCount = maxFood;
	}

	private void CreateFoodToPlayer(Player player)
	{
		Food newObj = Instantiate(foodPrefab);
		player.SetItem(newObj);
	}
	
	private void TransferFoodToPlayer(Food playerFood)
	{
		if (_food == null)
			return;
		while (playerFood.CurrentCount < playerFood.maxCount)
		{
			if (_currentFoodCount <= 0)
				break;
			playerFood.Increase();
			DecreaseFoodCount();
		}	
	}

	
	// TODO : Decrease힘수 빼고싶음
	private void DecreaseFoodCount()
	{
		if (_createFoodCoroutine == null)
			_createFoodCoroutine = StartCoroutine(CreateFoodCoroutine(createInterval));
		_currentFoodCount--;
		if (_currentFoodCount <= 0)
		{
			_currentFoodCount = 0;
			Destroy(_food.gameObject);
		}
	}
	
	private IEnumerator CreateFoodCoroutine(float interval)
	{
		while (_currentFoodCount < maxFood)
		{
			yield return new WaitForSeconds(interval);
			if (!isInteractable)
				continue;
			if (!_food) 
				Create();
			_currentFoodCount++;
		}
		_createFoodCoroutine = null;
	}

}
