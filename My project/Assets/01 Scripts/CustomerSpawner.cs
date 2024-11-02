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
	private Coroutine _spawnRoutine;

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
			if (_spawnRoutine == null)
				_spawnRoutine = StartCoroutine(CreateCustomerRoutine());
		}
	}
	
	private IEnumerator CreateCustomerRoutine()
	{
		yield return new WaitForSeconds(1f);
		CreateCustomer();
		_spawnRoutine = null;
	}
	
	
	private void CreateCustomer()
	{
		if (cashierDesk.customer)
			return;
		RaycastHit2D hit = Physics2D.Raycast(cashierDesk.transform.position, Vector2.down, 1f, LayerMask.GetMask(LayerName.Customer.ToString()));
		if (hit.collider != null)
			return;
		Create();
		cashierDesk.customer = GetCustomer();
	}
}
