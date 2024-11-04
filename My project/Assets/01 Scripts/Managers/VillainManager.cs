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
		currIndex = 2;
		Create();
		villain.MoveTo();

	}

	public void Create()
	{
		if (villain != null)
			return;
		villain = Instantiate(villainPrefab[currIndex]);
	}
}
