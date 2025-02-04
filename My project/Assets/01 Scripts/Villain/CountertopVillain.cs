using UnityEngine;
using Random = UnityEngine.Random;

public class CountertopVillain : Villain
{
    private InteractiveObject _interactiveObject;
    private bool isDestroy = false;
    protected override void Update()
    {
        if (isDestroy)
            return;
        base.Update();
        
        if (isDestroy)
            return;
        if (SearchInteractive(out InteractiveObject interactiveObject))
        {
            interactiveObject.isInteractable = false;
            _interactiveObject = interactiveObject;
        }
        
    }
    protected override void Destroy()
    {
        base.Destroy();
        isDestroy = true;
        if (_interactiveObject != null)
        {
            _interactiveObject.isInteractable = true;
        }
    }

    public override void MoveTo()
    {
        int objRan = Random.Range(0, 2);
        int dirRan = Random.Range(0, GameManager.Instance.countertops[objRan].interZones.Count);
        Vector3 tarPos = GameManager.Instance.countertops[objRan].transform.position;
        Vector3 dir = GameManager.Instance.countertops[objRan].interZones[dirRan].dir;
        if (dir.magnitude < 2.0f)
            dir *= GameManager.Instance.countertops[objRan].interZones[dirRan].rayDist;
        
        Vector3 pos = tarPos + dir;
        transform.position = pos;
    }
}
