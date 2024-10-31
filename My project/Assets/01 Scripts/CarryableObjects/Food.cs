using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Carryable
{
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
	
	public void Increase()
	{
		CurrentCount++;
	}
	
	public void DeCrease()
	{
		CurrentCount--;
	}

	public void Upgrade()
	{
		
	}
}
