using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
	public List<DiningTable> tables;
	public List<DiningTable> availableTables;

	public bool GetTable(out DiningTable outTable)
	{
		print("TableManager 1 : " + availableTables.Count);
		if (availableTables.Count == 0)
		{
			outTable = null;
			return false;
		}
		print("TableManager 2");
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
		tables.ForEach(table =>
			{
				if (table.IsAvailable() && !availableTables.Contains(table))
					availableTables.Add(table);
			}
		);
	}

	private void RetrieveOccupiedTable()
	{
		tables.ForEach
		(table =>
			{
				if ((table.isOccupied && table.guest == null))
					table.isOccupied = false;
			}
		);
	}
}
