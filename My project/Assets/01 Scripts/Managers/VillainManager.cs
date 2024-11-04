using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class VillainManager : Singleton<VillainManager>
{ 
	public Villain[] villainPrefab;
	public Villain villain;


	public void Create()
	{
		if (villain != null)
			return;
		villain = Instantiate(villainPrefab[0]);
	}
}
