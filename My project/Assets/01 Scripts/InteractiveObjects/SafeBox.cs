using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SafeBox : InteractiveObject
{
	public int CurrentMoney { get; private set; } = 0;

	private void Reset()
	{
		interZones = new List<InteractionZone>()
		{
			new InteractionZone {dir = Vector2.down, rayDist = 1f, layer = LayerName.Player},
			new InteractionZone {dir = Vector2.right, rayDist = 1f, layer = LayerName.Player},
		};
	}

	public void IncreaseMoney(int amount)
	{
		CurrentMoney += amount;
	}
	
	public void DecreaseMoney(int amount)
	{
		CurrentMoney -= amount;
		if (CurrentMoney < 0)
			CurrentMoney = 0;
	}
	
	public void HalveMoney()
	{
		CurrentMoney /= 2;
	}
	
	private bool _isUpgradePanelOpened = false;
	private void OpenUpgradePanel(Player player)
	{
		if (player == null)
			return;
		if (_isUpgradePanelOpened) return;
		UIManager.Instance.OpenUpgradePanel();
		_isUpgradePanelOpened = true;
	}

	private void TakeMoney(Player player)
	{
		if (player == null)
			return;
		if (player.carriedItem== null || player.carriedItem is not Money playerMoney)
			return;
		IncreaseMoney(playerMoney.CurrentCount);
		Destroy(playerMoney.gameObject);
	}

	private void Update()
	{
		DisplayRay();
		if (isInteractable == false)
			return;
		Player player = SearchPlayer();
		if (player != null)
		{
			Vector3 playerDir = player.transform.position - transform.position;
			if (playerDir == Vector3.right)
				OpenUpgradePanel(player);
			else if (playerDir == Vector3.down)
				TakeMoney(player);
			else
				_isUpgradePanelOpened = false;
		}
	}
}
