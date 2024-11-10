using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyUI;
    private void Start()
    {
        moneyUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        moneyUI.text = $" $ {GameManager.Instance.safeBox.CurrentMoney * 10}";
    }
}
