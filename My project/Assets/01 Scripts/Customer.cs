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

	private Collider2D _coll;
	
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


	private void Awake()
	{
		CurrentCount = 0;
		requiredCount = Random.Range(1, 3);
		_coll = GetComponent<Collider2D>();
	}

	private void Start()
	{
		food.spriteRenderer.enabled = false;
	}

	public void IncreaseFood()
	{
		food.Increase();
		CurrentCount++;
	}

	public void PickTable(DiningTable table)
	{ 
		this.table = table;
		table.isOccupied = true;
		table.customer = this;
	}

	public void GoToTable()
	{
		StartCoroutine(MoveRoutine());
		// transform.position = table.transform.position + Vector3.up;
	}

	private void Move(Vector2 dir)
	{
		if (!Mathf.Approximately(dir.magnitude, 1))
			return;
		Vector3 nextPosition = transform.position + (Vector3)dir;
		Physics2D.IgnoreCollision(_coll, _coll, true); // Raycast 전에 설정
		// RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, LayerMask.GetMask(
		// 	LayerName.Customer.ToString(), LayerName.Player.ToString(), LayerName.Villain.ToString()));
		//
		
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, 1f, LayerMask.GetMask(
			LayerName.Customer.ToString(), LayerName.Player.ToString(), LayerName.Villain.ToString()));
		//
		// print(hits.Length);
		// for (int i = 0; i < hits.Length; i++)
		// {
		// 	print(i + " =" + hits[i].collider);
		// }

		bool isHit = false;
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider == null || hit.collider.gameObject == gameObject)
				continue;
			isHit = true;
			break;
			// transform.position = nextPosition;
		}
		if (!isHit)
			transform.position = nextPosition;
		// RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, LayerMask.GetMask(LayerName.Player.ToString(), LayerName.Villain.ToString()));
		// print($"{hit.collider}, {hit.collider.GetInstanceID()} , {transform.GetInstanceID()}");
		// if (hit.collider == null || hit.collider.gameObject == gameObject)
		// {
		// 	transform.position = nextPosition;
		// }
		// else
		// {
		// 	// DiningTable 태그가 없음
		// 	// if (hit.collider.CompareTag(DiningTable))
		// 	// {
		// 	// 	hit.collider.GetComponent<DiningTable>().customer = this;
		// 	// 	PickTable(hit.collider.GetComponent<DiningTable>());
		// 	// }
		// }
	}

	private IEnumerator MoveRoutine()
	{
		Vector3 dest = table.transform.position + Vector3.up;
		while (!IsAtTable(dest))
		{
			if (transform.position.y > dest.y + 1)
				Move(Vector2.down);
			else if (dest.x > transform.position.x)
				Move(Vector2.right);
			else
				Move(Vector2.down);
			yield return new WaitForSeconds(1f);
		}
	}

	private bool IsAtTable(Vector3 dest)
	{
		return transform.position == dest;
	}
	
}
