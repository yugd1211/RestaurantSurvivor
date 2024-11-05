using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class VillainManager : Singleton<VillainManager>, Creatable
{
	private bool firstOneMinute = false;
	public Villain[] villainPrefab;
	private Villain _villain;
	private float _villainDeleteTime = 0f;
	
	public float cashierVillainCreateTime = 30f;
	public float countertopVillainCreateTime = 30f; 
	public float safeBoxVillainCreateTime = 30f;

	public Villain villain
	{
		get => _villain;
		set => _villain = value;
	}
	public int currIndex =  1;

	private void Start()
	{
		SetCreateRandomTime();
	}
	private void SetCreateRandomTime()
	{
		cashierVillainCreateTime = Random.Range(30f, 45f);
		countertopVillainCreateTime = Random.Range(30f, 45f);
		safeBoxVillainCreateTime = Random.Range(45f, 60f);
	}
	
	private IEnumerator firstOneMinuteCoroutine()
	{
		yield return new WaitForSeconds(60f);
		firstOneMinute = true;
	}
	
	private void Update()
	{
		if (villain != null)
			return;
		_villainDeleteTime += Time.deltaTime;
		if (_villainDeleteTime >= safeBoxVillainCreateTime)
		{
			for (int i = 0; i < villainPrefab.Length; i++)
			{
				if (villainPrefab[i] as SafeBoxVillain)
				{
					currIndex = i;
					Create();
					villain.MoveTo();
					return;
				}
			}
		}
		if (firstOneMinute || _villainDeleteTime >= cashierVillainCreateTime)
		{
			firstOneMinute = false;
			for (int i = 0; i < villainPrefab.Length; i++)
			{
				if (villainPrefab[i] is not CashierVillain || GameManager.Instance.cashierDesk.guest != null)
					continue;
				currIndex = i;

				Create();
				villain.MoveTo();
				return;
			}
		}
		if (firstOneMinute || _villainDeleteTime >= countertopVillainCreateTime)
		{
			firstOneMinute = false;
			for (int i = 0; i < villainPrefab.Length; i++)
			{
				if (villainPrefab[i] as CountertopVillain)
				{
					currIndex = i;
					Create();
					villain.MoveTo();
					return;
				}
			}
		}


	}

	public bool GetVillain<T>(out Villain villain)
	{
		if (this.villain)
		{ 
			villain = null;
			return false;
		}
		for (int i = 0; i < villainPrefab.Length; i++)
		{
			if (villainPrefab[i] is T)
			{
				currIndex = i;
				Create();
				villain = this.villain;
				return true;
			}
		}
		villain = null;
		return false;
	}

	public void Create()
	{
		if (villain != null)
			return;
		_villainDeleteTime = 0;
		SetCreateRandomTime();
		villain = Instantiate(villainPrefab[currIndex]);
	}
}
