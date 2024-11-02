using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour, Creatable
{
	public Customer customerPrefab;
	private Customer _currentCustomer;
	public CashierDesk cashierDesk;
	public float spawnTime = 1f;

	public void Create()
	{
		_currentCustomer = Instantiate(customerPrefab);
	}

	private void Start()
	{
		cashierDesk = FindObjectOfType<CashierDesk>();
	}

	public Customer GetCustomer()
	{
		Customer result = _currentCustomer;
		_currentCustomer = null;
		return result;
	}
	
	private void Update()
	{
		if (cashierDesk.customer == null)
		{
			StartCoroutine(CreateCustomerRoutine());
		}
	}
	
	private IEnumerator CreateCustomerRoutine()
	{
		yield return new WaitForSeconds(1f);
		CreateCustomer();
	}
	
	
	private void CreateCustomer()
	{
		if (cashierDesk.customer)
			return;
		Create();
		cashierDesk.customer = GetCustomer();
	}
}
