using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CountertopVillain : Villain
{
    private InteractiveObject _interactiveObject;
    // 조리대 기준 위치 랜덤으로 잡기

    protected override void Update()
    {
        base.Update();
        
        if (SearchInteractive(out InteractiveObject interactiveObject))
        {
            interactiveObject.isInteractable = false;
            _interactiveObject = interactiveObject;
        }
        
    }

    protected override void Destroy()
    {
        print(_interactiveObject);
        if (_interactiveObject != null) 
            _interactiveObject.isInteractable = true;
        base.Destroy();
    }

    public override void MoveTo()
    {
        int objRan = Random.Range(0, 2);
        int dirRan = Random.Range(0, GameManager.Instance.countertops[objRan].interZones.Count);
        Vector3 tarPos = GameManager.Instance.countertops[objRan].transform.position;
        Vector3 dir = GameManager.Instance.countertops[objRan].interZones[dirRan].dir * 
            GameManager.Instance.countertops[objRan].interZones[dirRan].rayDist;
        Vector3 pos = tarPos + dir;
        transform.position = pos;
    }
}
