using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CashierDesk : InteractiveObject, Creatable
{
	public Money prefab;
	public Money _money = null;
	private void Reset()
	{
		interZones = new List<InteractionZone>
		{
			new InteractionZone {dir = Vector2.left, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.up + Vector2.right, rayDist = 1f, layer = LayerName.Player},
		};
	}

	private void Start()
	{
		Create();
	}

	private void Update()
	{
		List<RaycastHit2D> hits = GetInteracObjsInRayPath();
		DisplayRay();
		hits.ForEach(item =>
		{
			if (item.transform != null)
			{
				if (item.transform.TryGetComponent(out Player player))
				{
					// 돈영역
					if (item.transform.position - transform.position == Vector3.left)
					{
						if (_money == null)
							return;
						Money playerMoney = player.carriedItem as Money;
						if (player.carriedItem == null)
						{
							player.carriedItem = _money;
							_money.transform.SetParent(player.transform);
							_money.transform.localPosition = Vector3.zero;
							_money = null;
						}
						else if (playerMoney != null && _money != null)
						{
							while (_money.CurrentCount > 0 && playerMoney.CurrentCount < playerMoney.maxCount)
							{
								playerMoney.Increase(); 
								_money.DeCrease();
							}
							if (_money.CurrentCount == 0)
								Destroy(_money.gameObject);

						}
					}
					else
					{
						// 손님 상호 작용 영역
					}
				}
			}

		});
	}
	
	
	public void Create()
	{
		if (!_money)
		{
			_money = Instantiate(prefab, transform.position + Vector3.left, Quaternion.identity, transform);
			_money.maxCount = 99;
		}
		_money.Increase();
	}
}
