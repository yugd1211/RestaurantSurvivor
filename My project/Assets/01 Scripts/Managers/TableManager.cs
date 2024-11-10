using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
	public List<DiningTable> tables;
	public List<DiningTable> availableTables;

	public bool GetTable(out DiningTable outTable )
	{
		outTable = null;
		if (availableTables.Count == 0)
			return false;
		outTable = availableTables[0];
		availableTables.RemoveAt(0);
		return true;
	}

	protected override void Awake()
	{
		base.Awake();
		UpdateAvailableTables();
	}

	private void Update()
	{
		UpdateAvailableTables();
	}

	private void UpdateAvailableTables()
	{
		RetrieveOccupiedTable();
    
		foreach (var table in tables)
		{
			if (table.IsAvailable() && !availableTables.Contains(table))
				availableTables.Add(table);
		}
	}

	private void RetrieveOccupiedTable()
	{
		foreach (DiningTable table in tables)
		{
			if (table.isOccupied && table.Guest == null)
				table.isOccupied = false;
		}
	}
}
