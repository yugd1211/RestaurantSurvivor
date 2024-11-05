using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
	public TextMeshProUGUI levelUI;

	private void Start()
	{
		levelUI = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Update()
	{
		levelUI.text =$"Lv.{GameManager.Instance.level + 1}";
	}

}
