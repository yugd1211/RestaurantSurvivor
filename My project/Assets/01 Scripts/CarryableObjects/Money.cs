using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Carryable
{
	public int price { get; private set; }
	public int CurrentCount { get; private set; }
	public int maxCount;

	private void Reset()
	{
		maxCount = 99;
		price = 10;
	}
	
	public void Increase()
	{
		CurrentCount++;
	}
	
	public void DeCrease()
	{
		CurrentCount--;
	}
	
}
