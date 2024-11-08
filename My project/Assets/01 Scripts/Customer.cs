using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour, CashierDeskInteractable, DiningTableInteractable
{
	// TODO : 다른 객체가 생성해줘야함
	public Food food;
	public DiningTable table;
	public int requiredCount;
	public Vector2Int requiredRange;

	private Collider2D _coll;

	public int CurrentCount { get; private set; }


	private void Awake()
	{
		CurrentCount = 0;
		requiredCount = Random.Range(requiredRange.x, requiredRange.y + 1);
		_coll = GetComponent<Collider2D>();
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
		table.guest = this;
	}

	public void GoToTable()
	{
		StartCoroutine(MoveRoutine());
	}

	public void Destroy()
	{
		if (table)
		{
			table.isOccupied = false;
			table.guest = null;
		}
		Destroy(gameObject);
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


		bool isHit = false;
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider == null || hit.collider.gameObject == gameObject)
				continue;
			isHit = true;
			break;
		}
		if (!isHit)
			transform.position = nextPosition;
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
		// Vector3 currentPosition = transform.position;
		//
		// float newX = currentPosition.x > dest.x ? dest.x : currentPosition.x;
		// float newY = currentPosition.y < dest.y ? dest.y : currentPosition.y;
		// transform.position = new Vector3(newX, newY);

		return transform.position == dest;
	}
	
}
