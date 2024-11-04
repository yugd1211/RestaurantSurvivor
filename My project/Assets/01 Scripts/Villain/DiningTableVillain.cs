using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DiningTableVillain : Villain, DiningTableInteractable
{
    public DiningTable diningTable;
    
    public override void MoveTo()
    {
        if (diningTable.isOccupied)
        {
            Destroy();
            return;
        }
        diningTable.isOccupied = true;
        diningTable.isInteractable = false;
        transform.position = diningTable.transform.position + Vector3.up;
    }
    
    protected override void Destroy()
    {
        if (diningTable != null) 
            diningTable.isInteractable = true;
        base.Destroy();
    }
}
