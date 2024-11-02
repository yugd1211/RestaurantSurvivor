using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Food : Carryable
{ 
	public SpriteRenderer spriteRenderer;
	public int startPrice;
	public int Price { get; private set; }
	public int CurrentCount { get; private set; }
	public int maxCount;

	private void Reset()
	{
		startPrice = 10;
		CurrentCount = 0;
		maxCount = 4;
	}

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Increase()
	{
		CurrentCount++;
	}
	
	public void DeCrease()
	{
		CurrentCount--;
		if (CurrentCount <= 0)
		{
			Destroy(gameObject);
		}
	}

	public void Upgrade()
	{
		
	}
}
