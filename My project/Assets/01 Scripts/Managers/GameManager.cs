using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public Player player;
	public int level;
	public bool isPause = false;
	public SafeBox safeBox;
	public CashierDesk cashierDesk;
	public Countertop[] countertops;

	protected override void Awake()
	{
		base.Awake();
		player = FindObjectOfType<Player>();
	}

	private void Start()
	{
		cashierDesk = FindObjectOfType<CashierDesk>(); 
		countertops = FindObjectsByType<Countertop>(FindObjectsSortMode.None);
	}

	public void Pause()
	{
		Time.timeScale = 0;
		isPause = true;
	}
	
	public void Resume()
	{
		Time.timeScale = 1;
		isPause = false;
	}
}
