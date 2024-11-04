using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class VillainManager : Singleton<VillainManager>
{ 
	public Villain[] villainPrefab;
	public Villain villain;
	public int currIndex =  1;


	private void Start()
	{
		currIndex = 3;
		Create();
		DiningTableVillain diningTableVillain = villain as DiningTableVillain;
		print(diningTableVillain);
		if (diningTableVillain == null)
			return;	
		if (TableManager.Instance.GetTable(out DiningTable table))
		{
			print(table);
			diningTableVillain.diningTable = table;
			villain.MoveTo();
		}

	}

	public void Create()
	{
		if (villain != null)
			return;
		villain = Instantiate(villainPrefab[currIndex]);
	}
}
