using TMPro;
using UnityEngine;

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

	public Type type; 
	private TextMeshProUGUI _priceText;

	private void Start()
	{
		_upgrade = FindObjectOfType<Upgrade>();
		_priceText = transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
		_priceText.text = "$: ";

		if (type == Type.Speed)
			_priceText.text += _upgrade.playerSpeedUpgradePrice.ToString();
		else if (type == Type.Storage)
			_priceText.text += _upgrade.playerStorageUpgradePrice.ToString();
		else if (type == Type.Villain)
			_priceText.text += _upgrade.playerVillainDefenseUpgradePrice.ToString();
		else if (type == Type.Countertop)
			_priceText.text += _upgrade.countertopUpgradePrice.ToString();
		else if (type == Type.Cashier)
			_priceText.text += _upgrade.cashierDeskUpgradePrice.ToString();
		else if (type == Type.DiningTable) 
			_priceText.text += _upgrade.diningTableUpgradePrice.ToString();
		_priceText.text += '0';
	}

	public void Upgrade()
	{
		if (type == Type.Speed)
			_upgrade.PlayerSpeedUpgrade();
		else if (type == Type.Storage)
			_upgrade.PlayerStorageUpgrade();
		else if (type == Type.Villain)
			_upgrade.PlayerVillainDefenseUpgrade();
		else if (type == Type.Countertop)
			_upgrade.CountertopUpgrade();
		else if (type == Type.Cashier)
			_upgrade.CashierDeskUpgrade();
		else if (type == Type.DiningTable) 
			_upgrade.DiningTableUpgrade();
	}
}
