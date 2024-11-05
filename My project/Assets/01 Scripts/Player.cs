using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
	public float moveSpeed;
	public Animator anim;
	private Coroutine _moveCoroutine; 
	private CashierTable _cashierTable;

	private Vector2 _moveDir;
	private bool _isMoving = true;
	public Carryable carriedItem;

	private void Reset()
	{
		moveSpeed = 2f;
		anim = GetComponent<Animator>();
		_cashierTable = FindObjectOfType<CashierTable>();
	}
	private void Start()
	{
		_moveCoroutine = StartCoroutine(CoMovePossible());
		_cashierTable = FindObjectOfType<CashierTable>();

	}

	private void Update()
	{
		_moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
		if (_isMoving)
			Move();
	}

	private void Move()
	{
		if (GameManager.Instance.isPause)
			return;
		if (_moveDir == Vector2.zero)
			return;
			// 방향에 따른 애니메이션 트리거 설정
			if (_moveDir == Vector2.up)
			{
				if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("PlayerUp"))
					anim.SetTrigger("UpTrigger");
			}
			else if (_moveDir == Vector2.down)
			{
				if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("PlayerDown"))
					anim.SetTrigger("DownTrigger");
			}
			else if (_moveDir == Vector2.left)
			{
				if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("PlayerLeft"))
					anim.SetTrigger("LeftTrigger");
			}
			else if (_moveDir == Vector2.right)
			{
				if (!anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("PlayerRight"))
					anim.SetTrigger("RightTrigger");
			}

			else
				return;
		if (!CheckPath(_moveDir))
			return;

		transform.position += new Vector3(_moveDir.x, _moveDir.y, 0);
		_isMoving = false;
	}

	bool CheckPath(Vector2 dir)
	{
		float rayDistance = 1f; // 레이 길이
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance,
			LayerMask.GetMask(LayerName.Interactive.ToString(), LayerName.Villain.ToString(), LayerName.Customer.ToString())); 
		if (hit.collider != null) 
			return false;
		return true;
	}

	private IEnumerator CoMovePossible()
	{
		while (true)
		{
			yield return null;
			if (_isMoving)
				continue;
			yield return new WaitForSeconds(1 / moveSpeed);
			_isMoving = true;
		}
	}

	public void PickUpCarriedItem()
	{
		
	}

	public void SetItem(Carryable item)
	{
		if (carriedItem)
		{
			if (item) DestroyImmediate(item.gameObject);
		}
		else
		{
			carriedItem = item;
			item.transform.SetParent(transform);
			item.transform.localPosition = new Vector3(0, -0.1f, 0);
		}
	}
}
