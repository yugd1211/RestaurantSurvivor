using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Villain : MonoBehaviour
{
	protected float DeleteTime = 3f;
	private Player _player;
	
	protected readonly Vector2[] SearchDirs = new Vector2[]
	{
		Vector2.up, Vector2.down, Vector2.left, Vector2.right, 
		Vector2.up + Vector2.left, Vector2.up + Vector2.right, Vector2.down + Vector2.left, Vector2.down + Vector2.right
	};
	private float _playerSearchTime = 0f;
	
	protected virtual void Update()
	{
		if (SearchPlayer())
			_playerSearchTime += Time.deltaTime;
		else
			_playerSearchTime = 0;
		if (DeleteTime / (_player == null ? 1 : _player.villainDefense) <= _playerSearchTime)
		{
			Destroy();
		}
	}

	protected virtual void Destroy()
	{
		VillainManager.Instance.villain = null;
		Destroy(gameObject);
	}
	
	public abstract void MoveTo();

	private bool SearchPlayer()
	{
		foreach (Vector2 dir in SearchDirs)
		{
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position, dir, 1f, 
				LayerMask.GetMask(LayerName.Player.ToString()));
			if (hit.collider)
			{
				_player = hit.collider.GetComponent<Player>();
				return true;
			}
		}
		return false;
	}
	
	protected bool SearchInteractive<T>(out T interactiveObject) where T : class
	{
		interactiveObject = null;
		foreach (Vector2 dir in SearchDirs)
		{
			RaycastHit2D hit = Physics2D.Raycast(
				transform.position, dir, 1f, 
				LayerMask.GetMask(LayerName.Interactive.ToString()));
			if (hit.collider && hit.collider.TryGetComponent(out interactiveObject))
				if (interactiveObject != null)
					return true;	
		}
		return false;
	}
}
