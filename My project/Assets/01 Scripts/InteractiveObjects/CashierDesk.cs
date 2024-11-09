using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CashierDesk : InteractiveObject, Creatable
{
	[SerializeField]
	private Money moneyPrefab; 
	private Money _money;
	
	public float saleSpeed = 0.5f;
	private Coroutine _saleCoroutine;
	private bool _isAutoSale = false;

	public CashierDeskInteractable guest;
	public CashierTable cashierTable;
	
	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up + Vector2.right, rayDist = 1f, layer = LayerName.Player},
		};
		saleSpeed = 0.5f;
	}

	private void Start()
	{
		cashierTable = FindObjectOfType<CashierTable>();
	}
	
	private void Update()
	{
		DisplayRay();
		if (!isInteractable)
			return;

		if (_isAutoSale && _saleCoroutine == null)
		{
			StartSaleCoroutine();
		}
		
		Player player = SearchPlayer();
		if (player != null)
			HandlePlayerInteraction(player);
	}

	
	public void Upgrade()
	{
		_isAutoSale = true;
		saleSpeed = 0.1f;
	}
	
	public void Create()
	{
		if (_money) 
			return;
		_money = Instantiate(moneyPrefab, transform.position + Vector3.left, Quaternion.identity, transform);
		_money.Init(99);
	}
	
	private void CreateMoneyToPlayer(Player player)
	{
		Money newObj = Instantiate(moneyPrefab);
		player.SetItem(newObj);
	}

	private void TransferMoneyToPlayer(Money playerMoney)
	{
		if (_money == null)
			return;
		while (playerMoney.CurrentCount < playerMoney.maxCount)
		{
			if (_money.CurrentCount <= 0)
				return;
			playerMoney.Increase(); 
			_money.DeCrease();
		}
	}
	
	private void HandleMoneyInteraction(Player player)
	{
		if (_money == null)
			return;
	
		if (player.carriedItem == null)
			CreateMoneyToPlayer(player);
		
		if (player.carriedItem is Money playerMoney)
			TransferMoneyToPlayer(playerMoney);
	}

	private void StartSaleCoroutine()
	{
		Customer customer = guest as Customer;
		if (customer == null || cashierTable == null || cashierTable.food == null || _saleCoroutine != null)
			return;
		_saleCoroutine = StartCoroutine(SaleRoutine(customer));
	}

	private void StopSaleCoroutine()
	{
		if (_saleCoroutine == null) 
			return;
		StopCoroutine(_saleCoroutine);
		_saleCoroutine = null;
	}

	private void HandlePlayerInteraction(Player player)
	{
		Vector3 playerDir = player.transform.position - transform.position;
		if (playerDir == Vector3.left)
			HandleMoneyInteraction(player);
		else if (!_isAutoSale)
		{
			if (playerDir == Vector3.up || playerDir == new Vector3(1, 1, 0))
				StartSaleCoroutine();
			else
				StopSaleCoroutine();
		}
	}
	
	private bool HasFood() => cashierTable.food?.CurrentCount > 0;
	private bool IsSaleComplete(Customer customer) => customer.CurrentCount < customer.requiredCount;

	private IEnumerator SaleRoutine(Customer customer)
	{
		while (IsSaleComplete(customer))
		{
			yield return new WaitForSeconds(saleSpeed);
			if (!HasFood())
			{
				cashierTable.food = null;
				continue;
			}
			if (_money == null)
				Create();
			Sale(customer);
		}

		if (customer.food.CurrentCount == customer.requiredCount)
		{
			StartCoroutine(SearchAvailableTableRoutine(customer));
		}

	}
	
	private bool TryGetAvailableTable(out DiningTable table) => TableManager.Instance.GetTable(out table);

	private void Sale(Customer customer)
	{
		_money.Increase();
		cashierTable.food.DeCrease();
		customer.IncreaseFood();
	}

	private IEnumerator SearchAvailableTableRoutine(Customer customer)
	{
		DiningTable table;
		yield return new WaitForSeconds(1f);
		while (!TryGetAvailableTable(out table))
			yield return new WaitForSeconds(1f);
		AssignTableToCustomer(customer, table);
		StopSaleCoroutine();
	}

	private void AssignTableToCustomer(Customer customer, DiningTable table)
	{
		if (customer == null || table == null || customer.food.CurrentCount < customer.requiredCount)
			return;
		customer.PickTable(table);
		customer.GoToTable();
		guest = null;
	}
}
