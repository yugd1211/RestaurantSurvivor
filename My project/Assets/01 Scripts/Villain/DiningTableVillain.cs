using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DiningTableVillain : Villain, DiningTableInteractable
{
    public DiningTable diningTable;
    
    private bool isDestroy = false;

    protected override void Update()
    {
        if(isDestroy)
            return;
        base.Update();
    }

    public override void MoveTo()
    {
        print("MoveTo Before");

        if (diningTable.isOccupied)
        {
            Destroy();
            return;
        }
        print("MoveTo After");
        diningTable.isOccupied = true;
        diningTable.isInteractable = false;
        transform.position = diningTable.transform.position + Vector3.up;
    }
    
    protected override void Destroy()
    {
        isDestroy = true;
        if (diningTable != null) 
            diningTable.isInteractable = true;
        base.Destroy();
    }
}
