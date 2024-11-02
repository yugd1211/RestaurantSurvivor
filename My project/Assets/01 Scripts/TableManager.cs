using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
	public List<DiningTable> tables;

	public bool GetTable(out DiningTable outTable)
	{
		foreach (DiningTable table in tables)
		{
			if (table.isOccupied)
				continue;
			outTable = table;
			return true;
		}
		outTable = null;
		return false;
	}

	private void Update()
	{
		RetrieveOccupiedTable();
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
