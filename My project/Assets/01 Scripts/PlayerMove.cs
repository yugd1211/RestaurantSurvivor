using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
	public float moveSpeed;
	public Animator anim;
	
	
	private Coroutine _moveCoroutine;
	private Vector2 _moveDir;
	private bool _isMoving = true;

	private void Reset()
	{
		moveSpeed = 2f;
		anim = GetComponent<Animator>();
	}
	private void Start()
	{
		_moveCoroutine = StartCoroutine(CoMovePossible());
	}

	private void Update()
	{
		_moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		if (_isMoving)
			Move();
	}

	private void Move()
	{
		if (_moveDir == Vector2.zero)
			return;
			// 방향에 따른 애니메이션 트리거 설정
			if (_moveDir == Vector2.up)
				anim.SetTrigger("UpTrigger");
			else if (_moveDir == Vector2.down)
				anim.SetTrigger("DownTrigger");
			else if (_moveDir == Vector2.left)
				anim.SetTrigger("LeftTrigger");
			else if (_moveDir == Vector2.right)
				anim.SetTrigger("RightTrigger");
		if (CheckPath(_moveDir))
			return;

		transform.position += new Vector3(_moveDir.x, _moveDir.y, 0);
		_isMoving = false;
	}

	bool CheckPath(Vector2 dir)
	{
		float rayDistance = 1f; // 레이 길이
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance, LayerMask.GetMask("Interactive")); 
		if (hit.collider != null) 
			return true;
		return false;
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

}
