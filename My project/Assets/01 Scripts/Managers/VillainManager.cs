using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class VillainManager : Singleton<VillainManager>, Creatable
{ 
	public Villain[] villainPrefab;
	private Villain _villain;
	private float _villainDeleteTime = 0f;
	public Villain villain
	{
		get
		{
			return _villain;
		}
		set
		{
			if (value == null)
			{
				_villainDeleteTime = 0;
			}
			_villain = value;
		}
	}
	public int currIndex =  1;

	private void Update()
	{
		_villainDeleteTime += Time.deltaTime;
		// print($"delete time = {_villainDeleteTime}");
	}

	public bool GetVillain<T>(out Villain villain)
	{
		// print("VictimManager GetVillain");
		if (this.villain)
		{ 
			// print("VictimManager this.villain = true");
			villain = null;
			return false;
		}
		// print("VictimManager before for");
		for (int i = 0; i < villainPrefab.Length; i++)
		{
			if (villainPrefab[i] is T)
			{
				// print("GetVillain");
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
		villain = Instantiate(villainPrefab[currIndex]);
	}
}
