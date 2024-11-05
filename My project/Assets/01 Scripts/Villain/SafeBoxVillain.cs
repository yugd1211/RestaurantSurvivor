using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxVillain : Villain
{
    public float safeBoxDeleteTime = 30f;
    private float _safeBoxSearchTime = 0f;
    private bool isDestroy = false;
    
    public override void MoveTo()
    {
        transform.position = GameManager.Instance.safeBox.transform.position + Vector3.down;
    }

    protected override void Update()
    {
        if (isDestroy) 
            return;
        base.Update();

        if (SearchSafeBox(out SafeBox safeBox))
        {
            safeBox.isInteractable = false;
            _safeBoxSearchTime += Time.deltaTime;
        }
        else
        {
            _safeBoxSearchTime = 0;
            
        }
        if (_safeBoxSearchTime >= safeBoxDeleteTime)
        {
            safeBox.HalveMoney();
            Destroy(gameObject);
        }
    }

    private bool SearchSafeBox(out SafeBox safeBox)
    {
        safeBox = null;
        foreach (Vector2 dir in SearchDirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, dir, 1f, 
                LayerMask.GetMask(LayerName.Interactive.ToString()));
            if (hit.collider.TryGetComponent(out safeBox))
                return true;
        }
        return false;
    }
}
