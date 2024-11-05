using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierVillain : Villain, CashierDeskInteractable
{
    private bool isDestroy = false;

    protected override void Update()
    {
        if (isDestroy) 
            return;
        base.Update();
    }
    public override void MoveTo()
    {
        if (GameManager.Instance.cashierDesk.guest != null)
        {
            Destroy();
            return;
        }
        GameManager.Instance.cashierDesk.isInteractable = false;
        transform.position = GameManager.Instance.cashierDesk.transform.position + Vector3.down;
        GameManager.Instance.cashierDesk.guest = this;
    }

    protected override void Destroy()
    {
        isDestroy = true;
        base.Destroy();
        if (GameManager.Instance.cashierDesk.guest != this)
            return;
        GameManager.Instance.cashierDesk.isInteractable = true;
        GameManager.Instance.cashierDesk.guest = null;
    }
}
