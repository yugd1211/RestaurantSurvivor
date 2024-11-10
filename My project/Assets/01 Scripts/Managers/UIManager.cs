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
		warningUI = GameObject.Find("WarningUI");
		
		GameObject.Find("ResumePanel").GetComponentInChildren<Button>().onClick.AddListener(Resume);
		GameObject.Find("GameOverPanel").GetComponentInChildren<Button>().onClick.AddListener(GameOver);
		GameObject.Find("PauseUI").GetComponentInChildren<Button>().onClick.AddListener(Pause);
		
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
		SetWarningUI(VillainManager.Instance.villain != null);
	}
	
	private void SetWarningUI(bool isActive)
	{
		if (warningUI != null)
			warningUI.SetActive(isActive);
	}
	
	private void Pause()
	{
		GameManager.Instance.Pause();
		pausePanel.gameObject.SetActive(true);
	}
	
	public void OpenUpgradePanel()
	{
		GameManager.Instance.Pause();
		updgadePanel.SetActive(true);
	}
	
	private void Resume()
	{
		GameManager.Instance.Resume();
		CloseAllPanel();
	}

	private void CloseAllPanel()
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
