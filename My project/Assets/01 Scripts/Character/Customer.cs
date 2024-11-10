using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour, CashierDeskInteractable, DiningTableInteractable
{
	public Food food;
	public DiningTable table;
	public int requiredCount;
	public Vector2Int requiredRange;

	public int CurrentCount { get; private set; }

	private void Awake()
	{
		CurrentCount = 0;
		requiredCount = Random.Range(requiredRange.x, requiredRange.y + 1);
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
		table.Guest = this;
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
			table.Guest = null;
		}
		Destroy(gameObject);
	}

	private void Move(Vector2 dir)
	{
		if (!Mathf.Approximately(dir.magnitude, 1))
			return;
		
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, 1f, LayerMask.GetMask(
			LayerName.Customer.ToString(), LayerName.Player.ToString(), LayerName.Villain.ToString()));

		if (!hits.Any(hit => hit.collider != null && hit.collider.gameObject != gameObject))
			transform.position += (Vector3)dir;
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
		Vector3 currentPosition = transform.position;
		
		float newX = currentPosition.x > dest.x ? dest.x : currentPosition.x;
		float newY = currentPosition.y < dest.y ? dest.y : currentPosition.y;
		transform.position = new Vector3(newX, newY);

		return transform.position == dest;
	}
	
}
