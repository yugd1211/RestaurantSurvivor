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
        base.Destroy();
        isDestroy = true;
        if (diningTable != null)
        {
            diningTable.isOccupied = false;
            diningTable.isInteractable = true;
        }
    }
}
