using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	public GameObject pausePanel;
	public GameObject updgadePanel;
	public GameObject warningUI;

	private void Start()
	{
		pausePanel = GameObject.Find("PausePanel");
		updgadePanel = GameObject.Find("UpgradePanel");
		GameObject.Find("ResumePanel").GetComponentInChildren<Button>().onClick.AddListener(Resume);
		GameObject.Find("GameOverPanel").GetComponentInChildren<Button>().onClick.AddListener(GameOver);
		GameObject.Find("PauseUI").GetComponentInChildren<Button>().onClick.AddListener(Pause);
		warningUI = GameObject.Find("WarningUI");
		warningUI.SetActive(false);
		pausePanel.SetActive(false);
		updgadePanel.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameManager.Instance.isPause)
				Resume();
			else
				Pause();
		}
		if (VillainManager.Instance.villain)
			warningUI.SetActive(true);
		else
			warningUI.SetActive(false);
	}
	
	private void Pause()
	{
		GameManager.Instance.isPause = true;
		Time.timeScale = 0;
		pausePanel.gameObject.SetActive(true);
	}
	
	public void OpenUpgradePanel()
	{
		// print("OpenUpgradePanel");
		GameManager.Instance.isPause = true;
		updgadePanel.SetActive(true);
		Time.timeScale = 0;
	}
	
	private void Resume()
	{
		GameManager.Instance.isPause = false;
		Time.timeScale = 1;
		CloesePanel();
	}

	public void CloesePanel()
	{
		updgadePanel.SetActive(false);
		pausePanel.SetActive(false);
	}
	
	private void GameOver()
	{
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
		        Application.Quit();
		#endif
	}
}
