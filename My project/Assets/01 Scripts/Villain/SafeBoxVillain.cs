using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxVillain : Villain
{
    public float safeBoxDeleteTime = 30f;
    private float _safeBoxSearchTime = 0f;
    private bool isDestroy = false;

    private void Start()
    {
        DeleteTime = 1.5f;
    }

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
    
    protected override void Destroy()
    {
        isDestroy = true;
        GameManager.Instance.safeBox.isInteractable = true;
        base.Destroy();
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
