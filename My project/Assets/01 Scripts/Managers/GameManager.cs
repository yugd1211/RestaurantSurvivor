using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public Player player;
	public int level;
	public bool isPause = false;
	public SafeBox safeBox;
	public CashierDesk cashierDesk;
	public CustomerSpawner customerSpawner; 
	public Countertop[] countertops;

	protected override void Awake()
	{
		base.Awake();
		customerSpawner = GetComponent<CustomerSpawner>();
		player = FindObjectOfType<Player>();
	}

	private void Start()
	{
		cashierDesk = FindObjectOfType<CashierDesk>(); 
		countertops = FindObjectsByType<Countertop>(FindObjectsSortMode.None);
	}
}
