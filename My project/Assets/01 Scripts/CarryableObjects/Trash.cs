using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : Carryable
{	
	public int CurrentCount { get; private set; }
	public int maxCount;

	private void Reset()
	{
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
