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
		// foreach (DiningTable table in tables)
		// {
		// 	if (!table.IsAvailable())
		// 		continue;
		// 	outTable = table;
		// 	return true;
		// }
		// outTable = null;
		// return false;
		if (availableTables.Count == 0)
		{
			outTable = null;
			return false;
		}
		outTable = availableTables[0];
		availableTables.RemoveAt(0);
		return true;
	}

	private void Update()
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
				if (table.isOccupied && table.customer == null)
					table.isOccupied = false;
			}
		);
		
	}
}
