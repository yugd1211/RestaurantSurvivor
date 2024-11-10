using UnityEngine;
using Random = UnityEngine.Random;

public class VillainManager : Singleton<VillainManager>, Creatable
{
	public Villain[] villainPrefab;
	public float cashierVillainCreateTime = 30f;
	public float countertopVillainCreateTime = 30f; 
	public float safeBoxVillainCreateTime = 30f;

	public Villain villain;
	private float _villainDeleteTime = 0f;
	public int currIndex =  1;

	private void Start()
	{
		_villainDeleteTime = -30;
		cashierVillainCreateTime = 30;
		countertopVillainCreateTime = 30;
		safeBoxVillainCreateTime = int.MaxValue;
	}
	
	private void ResetVillainCreateTimes()
	{
		cashierVillainCreateTime = Random.Range(30f, 45f);
		countertopVillainCreateTime = Random.Range(30f, 45f);
		safeBoxVillainCreateTime = Random.Range(30f, 45f);
	}
	
	private void Update()
	{
		if (villain != null)
			return;
		_villainDeleteTime += Time.deltaTime;
		if (_villainDeleteTime >= safeBoxVillainCreateTime && TrySpawnVillain<SafeBoxVillain>())
			return;
		if (_villainDeleteTime >= cashierVillainCreateTime && GameManager.Instance.cashierDesk.guest == null && TrySpawnVillain<CashierVillain>())
			return;
		if (_villainDeleteTime >= countertopVillainCreateTime && TrySpawnVillain<CountertopVillain>())
			return;
	}
	
	private void CreateAndMoveVillain(int index)
	{
		currIndex = index;
        Create();
        villain.MoveTo();
    }
	
	private bool TrySpawnVillain<T>() where T : Villain
	{
		for (int i = 0; i < villainPrefab.Length; i++)
		{
			if (villainPrefab[i] is T)
			{
				CreateAndMoveVillain(i);
				return true;
			}
		}
		return false;
	}

	public bool GetVillain<T>(out Villain villain)
	{
		villain = null;
		if (this.villain)
			return false;
		for (int i = 0; i < villainPrefab.Length; i++)
		{
			if (villainPrefab[i] is T)
			{
				currIndex = i;
				Create();
				villain = this.villain;
				return true;
			}
		}
		return false;
	}

	public void Create()
	{
		if (villain != null)
			return;
		_villainDeleteTime = 0;
		ResetVillainCreateTimes();
		villain = Instantiate(villainPrefab[currIndex]);
	}
}
