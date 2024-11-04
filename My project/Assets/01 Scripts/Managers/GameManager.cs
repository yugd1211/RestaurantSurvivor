using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{ 
	public SafeBox safeBox;
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
