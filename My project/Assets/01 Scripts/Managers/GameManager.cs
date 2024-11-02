using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{
	public CashierDesk cashierDesk;
	public CustomerSpawner customerSpawner;

	protected override void Awake()
	{
		base.Awake();
		customerSpawner = GetComponent<CustomerSpawner>();
	}

	private void Start()
	{
		cashierDesk = FindObjectOfType<CashierDesk>();
	}

}
