using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
	public GameObject cursor;
	
	public Button[] buttons;

	public Button currentButton;

	private void Update()
	{
		cursor.transform.position = currentButton.transform.position;

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			currentButton = buttons[0];
		else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			currentButton = buttons[1];
		else if (Input.GetKeyDown(KeyCode.Return))
			currentButton.onClick.Invoke();
	}
}
