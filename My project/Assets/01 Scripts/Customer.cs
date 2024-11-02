using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
	// TODO : 다른 객체가 생성해줘야함
	public Food food;
	public DiningTable table;
	public int requiredCount;
	
	private int _currentCount;
	public int CurrentCount 
	{
		get => _currentCount;
		private set
		{
			_currentCount = value;
			if (_currentCount == 0)
			{
				if (food)
					food.spriteRenderer.enabled = false;
			}
			else
			{
				if (food)
					food.spriteRenderer.enabled = true;
			}
		} 
	}


	private void Start()
	{
		food.spriteRenderer.enabled = false;
	}

	public void FindTable()
	{
		// table = FindObjectOfType<DiningTable>();
	}

	public void IncreaseFood()
	{
		food.Increase();
		CurrentCount++;
	}

	private void Awake()
	{
		CurrentCount = 0;
		requiredCount = Random.Range(1, 3);
	}

	public void PickTable(DiningTable table)
	{ 
		this.table = table;
		table.isOccupied = true;
		table.customer = this;
	}

	public void GoToTable()
	{
		transform.position = table.transform.position + Vector3.up;
	}

	private void Move(Vector2 dir)
	{
		if (!Mathf.Approximately(dir.magnitude, 1))
			return;
		Vector3 nextPosition = transform.position + (Vector3)dir;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, LayerMask.GetMask("Interactive"));
		if (hit.collider == null)
		{
			transform.position = nextPosition;
		}
		else
		{
			if (hit.collider.CompareTag("DiningTable"))
			{
				// hit.collider.GetComponent<DiningTable>().customer = this;
				// PickTable(hit.collider.GetComponent<DiningTable>());
			}
		}
	}
}
