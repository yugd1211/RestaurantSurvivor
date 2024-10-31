using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Carryable
{
	public int startPrice;
	public int Price { get; private set; }

	private void Reset()
	{
		startPrice = 10;
	}

	public void Upgrade()
	{
		
	}
}
