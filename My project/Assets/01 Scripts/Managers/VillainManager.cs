using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class VillainManager : Singleton<VillainManager>, Creatable
{
	public Villain[] villainPrefab;
	public int firstSpawn = 60;
	public float cashierVillainCreateTime = 30f;
	public float countertopVillainCreateTime = 30f; 
	public float safeBoxVillainCreateTime = 30f;

	private bool _isFirstSpawn = false;
	private Villain _villain;
	private float _villainDeleteTime = 0f;
	public Villain villain
	{
		get => _villain;
		set => _villain = value;
	}
	public int currIndex =  1;

	private void Start()
	{
		SetCreateRandomTime();
		StartCoroutine(FirstOneMinuteCoroutine());
	}
	private void SetCreateRandomTime()
	{
		cashierVillainCreateTime = Random.Range(30f, 45f);
		countertopVillainCreateTime = Random.Range(30f, 45f);
		safeBoxVillainCreateTime = Random.Range(45f, 60f);
	}
	
	private IEnumerator FirstOneMinuteCoroutine()
	{ 
		yield return new WaitForSeconds(firstSpawn);
		if (_isFirstSpawn)
			yield break;
		int index = Random.Range(1, 3);
		Create();
		villain.MoveTo();
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
		if (_villainDeleteTime >= cashierVillainCreateTime)
		{
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
		if (_villainDeleteTime >= countertopVillainCreateTime)
		{
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
		_isFirstSpawn = true;
		SetCreateRandomTime();
		villain = Instantiate(villainPrefab[currIndex]);
	}
}
