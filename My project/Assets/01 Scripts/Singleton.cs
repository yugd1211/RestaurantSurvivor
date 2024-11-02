using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
	
	private static Singleton _instance;
	public static Singleton Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<Singleton>();
				if (_instance == null)
				{
					GameObject go = new GameObject("Singleton");
					_instance = go.AddComponent<Singleton>();
				}
			}
			return _instance;
		}
	}
	
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}
}
