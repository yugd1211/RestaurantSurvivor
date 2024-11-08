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

	private void Reset()
	{
		startPrice = 10;
	}

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
	}

	public override void Increase()
	{
		base.Increase();
		spriteRenderer.enabled = true;
	}
	
	public override void Init(int maxCount = 4)
	{
		base.Init(maxCount);
		Price =	startPrice;
	}
}
