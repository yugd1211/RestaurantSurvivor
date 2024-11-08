using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour, Creatable
{
	public Customer[] customerPrefab;
	private Customer _currentCustomer;
	public CashierDesk cashierDesk;
	public float spawnTime = 1f;
	private Coroutine _spawnRoutine;
	private int[] _levelTable = { 100, 100, 90, 90, 90, 80, 80, 80, 60, 60, 40, 40, 20 };
	
	public void Create()
	{
		int ran = Random.Range(0, 100) < _levelTable[GameManager.Instance.level] ? 0 : 1;
		_currentCustomer = Instantiate(customerPrefab[ran]);
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
		if (cashierDesk.guest == null)
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
		if (cashierDesk.guest != null)
			return;
		RaycastHit2D hit = Physics2D.Raycast(cashierDesk.transform.position, Vector2.down, 1f, LayerMask.GetMask(LayerName.Customer.ToString()));
		if (hit.collider != null)
			return;
		Create();
		cashierDesk.guest = GetCustomer();
	}
}
