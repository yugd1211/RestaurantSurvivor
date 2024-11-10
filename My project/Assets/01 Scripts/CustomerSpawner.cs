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
	private readonly int[] _levelTable = { 100, 100, 90, 90, 90, 80, 80, 80, 60, 60, 40, 40, 20 };
	
	private void Start()
	{
		cashierDesk = FindObjectOfType<CashierDesk>();
	}
	
	private void Update()
	{
		if (cashierDesk.guest == null && _currentCustomer == null)
			StartCoroutine(CreateCustomerRoutine());
	}
	
	public void Create()
	{
		int ran = Random.Range(0, 100) < _levelTable[GameManager.Instance.level] ? 0 : 1;
		_currentCustomer = Instantiate(customerPrefab[ran]);
	}

	private IEnumerator CreateCustomerRoutine()
	{
		yield return new WaitForSeconds(1f);
		CreateCustomer();
	}

	private void CreateCustomer()
	{
		if (cashierDesk.guest != null)
			return;
		RaycastHit2D hit = Physics2D.Raycast(cashierDesk.transform.position, Vector2.down, 1f, 
			LayerMask.GetMask(LayerName.Customer.ToString(), LayerName.Villain.ToString(), LayerName.Player.ToString()));
		if (hit.collider != null)
			return;
		Create();
		cashierDesk.guest = _currentCustomer;
		_currentCustomer = null;
	}
}
