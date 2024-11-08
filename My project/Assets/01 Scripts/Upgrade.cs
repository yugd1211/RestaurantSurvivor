using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
	
	public int playerSpeedUpgradePrice = 30; 
	public int playerStorageUpgradePrice = 30;
	public int playerVillainDefenseUpgradePrice = 30; 
	public int countertopUpgradePrice = 20;
	public int cashierDeskUpgradePrice = 50;
	public int diningTableUpgradePrice = 30;


	private bool _isPlayerSpeedUpgrade = false;
	private bool _isPlayerStorageUpgrade = false;
	private bool _isPlayerVillainDefenseUpgrade = false;
	private bool _isCountertopUpgrade = false;
	private bool _isCashierDeskUpgrade = false;
	private bool _isDiningTableUpgrade = false;

	private void Start()
	{
		playerSpeedUpgradePrice = 1;
			playerStorageUpgradePrice = 1;
		playerVillainDefenseUpgradePrice = 1;
			countertopUpgradePrice = 1;
		cashierDeskUpgradePrice = 1;
			diningTableUpgradePrice = 1;
	}

	public void PlayerSpeedUpgrade()
	{
		if (_isPlayerSpeedUpgrade || GameManager.Instance.safeBox.CurrentMoney < playerSpeedUpgradePrice)
			return;
		GameManager.Instance.safeBox.DecreaseMoney(playerSpeedUpgradePrice);
		_isPlayerSpeedUpgrade = true;
		GameManager.Instance.level++;
		GameManager.Instance.player.moveSpeed *= 2;
	}
	
	public void PlayerStorageUpgrade()
	{
		if (_isPlayerStorageUpgrade || GameManager.Instance.safeBox.CurrentMoney < playerStorageUpgradePrice)
			return;
        GameManager.Instance.safeBox.DecreaseMoney(playerStorageUpgradePrice);
		_isPlayerStorageUpgrade = true;
		GameManager.Instance.level++;
		GameManager.Instance.player.maxStorage = 10;
	}
	
	public void PlayerVillainDefenseUpgrade()
	{
		if (_isPlayerVillainDefenseUpgrade || GameManager.Instance.safeBox.CurrentMoney < playerVillainDefenseUpgradePrice)
			return;
        GameManager.Instance.safeBox.DecreaseMoney(playerVillainDefenseUpgradePrice);
		_isPlayerVillainDefenseUpgrade = true;
        GameManager.Instance.level++;
		GameManager.Instance.player.villainDefense = 2;
	}
	
	public void CountertopUpgrade()
	{
		if (_isCountertopUpgrade || GameManager.Instance.safeBox.CurrentMoney < countertopUpgradePrice)
			return;
        GameManager.Instance.safeBox.DecreaseMoney(countertopUpgradePrice);
		_isCountertopUpgrade = true;
        GameManager.Instance.level++;
        foreach (Countertop c in GameManager.Instance.countertops)
        {
	        c.Upgrade();
        }
	}

	public void CashierDeskUpgrade()
	{
		if (_isCashierDeskUpgrade || GameManager.Instance.safeBox.CurrentMoney < cashierDeskUpgradePrice)
			return;
        GameManager.Instance.safeBox.DecreaseMoney(cashierDeskUpgradePrice);
		_isCashierDeskUpgrade = true;
        GameManager.Instance.level++;
		GameManager.Instance.cashierDesk.Upgrade();
	}
	
	public void DiningTableUpgrade()
	{
		if (_isDiningTableUpgrade || GameManager.Instance.safeBox.CurrentMoney < diningTableUpgradePrice)
			return;
        GameManager.Instance.safeBox.DecreaseMoney(diningTableUpgradePrice);
		_isDiningTableUpgrade = true;
        GameManager.Instance.level++;
		TableManager.Instance.tables.ForEach(item =>
		{
			item.deleteCount *= 2;
		});
	}
}
