using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
	private Upgrade _upgrade;
	public enum Type
	{
		Speed,
		Storage,
		Villain,
		Countertop,
		Cashier,
		DiningTable,
	}
	private TextMeshProUGUI _priceText;
	
	public Type type; 

	private void Start()
	{
		_upgrade = FindObjectOfType<Upgrade>();
		_priceText = transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
		_priceText.text = "$: ";

		switch (type)
		{
			case Type.Speed:
				_priceText.text += _upgrade.playerSpeedUpgradePrice.ToString();
				break;
			case Type.Storage:
				_priceText.text += _upgrade.playerStorageUpgradePrice.ToString();
				break;
			case Type.Villain:
				_priceText.text += _upgrade.playerVillainDefenseUpgradePrice.ToString();
				break;
			case Type.Countertop:
				_priceText.text += _upgrade.countertopUpgradePrice.ToString();
				break;
			case Type.Cashier:
				_priceText.text += _upgrade.cashierDeskUpgradePrice.ToString();
				break;
			case Type.DiningTable:
				_priceText.text += _upgrade.diningTableUpgradePrice.ToString();
				break;
		}
		_priceText.text += '0';
		// _priceText.text = _upgrade
	}

	public void Upgrade()
	{
		switch (type)
		{
			case Type.Speed:
				_upgrade.PlayerSpeedUpgrade();
				break;
			case Type.Storage:
				_upgrade.PlayerStorageUpgrade();
				break;
			case Type.Villain:
				_upgrade.PlayerVillainDefenseUpgrade();
				break;
			case Type.Countertop:
				_upgrade.CountertopUpgrade();
				break;
			case Type.Cashier:
				_upgrade.CashierDeskUpgrade();
				break;
			case Type.DiningTable:
				_upgrade.DiningTableUpgrade();
				break;
		}
	}
}
