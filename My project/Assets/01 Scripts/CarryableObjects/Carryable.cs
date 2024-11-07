using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
	public int CurrentCount { get; protected set; }
	public int maxCount;
	
	public virtual void Init(int maxCount = 4)
	{
		this.maxCount = maxCount;
		CurrentCount = 0;
	}
	
	public void Increase()
	{
		CurrentCount++;
	}
	
	public void DeCrease()
	{
		CurrentCount--;
		if (CurrentCount <= 0)
		{
			Destroy(gameObject);
		}
	}
}
