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
	
	public virtual void Increase()
	{
		CurrentCount++;
	}
	
	public virtual void Decrease()
	{
		CurrentCount--;
		if (CurrentCount <= 0)
			Destroy(gameObject);
	}
}
