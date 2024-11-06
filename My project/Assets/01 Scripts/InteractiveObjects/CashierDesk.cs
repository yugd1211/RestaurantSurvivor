using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierDesk : InteractiveObject, Creatable
{
	public Money prefab; 
	public Money money = null;
	public CashierDeskInteractable guest = null;
	public float saleSpeed = 0.5f;
	private CashierTable _cashierTable;
	private Coroutine _saleCoroutine;
	private bool isAutoSale = false;
	
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
	
	public void OnAutoSale()
	{
		isAutoSale = true;
	}
	
	public void SetCustomer(Customer customer)
	{
		this.guest = customer;
		customer.transform.SetParent(transform);
		customer.transform.position = transform.position + Vector3.down;
		// guest.transform.localPosition = Vector3.down;
	}

	private void Start()
	{
		_cashierTable = FindObjectOfType<CashierTable>();
	}

	private void Update()
	{
		DisplayRay();
		if (isInteractable == false)
			return;
		if (isAutoSale)
			_saleTime += Time.deltaTime;
		List<RaycastHit2D> hits = GetInteracObjsInRayPath();
		hits.ForEach(item =>
		{
			if (item.transform != null)
			{
				if (item.transform.TryGetComponent(out Player player))
				{
					// 돈영역
					if (item.transform.position - transform.position == Vector3.left)
					{
						if (money == null)
							return;
						Money playerMoney = player.carriedItem as Money;
						if (player.carriedItem == null)
						{
							player.carriedItem = money;
							money.transform.SetParent(player.transform);
							money.transform.localPosition = Vector3.zero;
							money = null;
						}
						else if (playerMoney != null && money != null)
						{
							while (money.CurrentCount > 0 && playerMoney.CurrentCount < playerMoney.maxCount)
							{
								playerMoney.Increase(); 
								money.DeCrease();
							}
							if (money.CurrentCount == 0)
								Destroy(money.gameObject);

						}
					}
					// 손님 상호 작용 영역
					else if (!isAutoSale && (item.transform.position - transform.position == Vector3.up ||
					                        item.transform.position - transform.position == Vector3.up + Vector3.right))
					{
						if (this.guest == null || _cashierTable == null || _cashierTable.food == null)
							return;
						_saleTime += Time.deltaTime;
						if (_saleCoroutine == null)
							_saleCoroutine = StartCoroutine(SaleRoutine());
					}
					else
						_saleTime = 0;
				}
			}
		});
	}

	private float _saleTime = 0f;

	private IEnumerator SaleRoutine()
	{
		Customer guest = this.guest as Customer;
		while (guest.CurrentCount < guest.requiredCount)
		{
			if (_saleTime < saleSpeed)
			{
				yield return null;
				continue;
			}
			_saleTime = 0;
			if (_cashierTable.food.CurrentCount <= 0)
				_cashierTable.food = null;
			if (money == null)
				Create();
			money.Increase();
			_cashierTable.food.DeCrease();
			guest.IncreaseFood();
		}
		if (guest.food.CurrentCount == guest.requiredCount)
			StartCoroutine(SearchAvailableTableRoutine());
		_saleCoroutine = null;
	}

	private IEnumerator SearchAvailableTableRoutine()
	{
		DiningTable table = null;
		while (true)
		{
			if (TableManager.Instance.GetTable(out table))
				break;
			yield return new WaitForSeconds(1f);
		}
		Customer guest = this.guest as Customer;
		if (this.guest == null)
			yield break;
		guest.PickTable(table);
		guest.GoToTable();
		this.guest = null;
		StopAllCoroutines();
	}
	
	public void Create()
	{
		if (!money)
		{
			money = Instantiate(prefab, transform.position + Vector3.left, Quaternion.identity, transform);
			money.maxCount = 99;
		}
		money.Increase();
	}
}
