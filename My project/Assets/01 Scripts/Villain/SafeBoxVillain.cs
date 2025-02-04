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

        if (SearchInteractive(out SafeBox safeBox))
        {
            if (isDestroy) 
                return;
            safeBox.isInteractable = false;
            _safeBoxSearchTime += Time.deltaTime;
        }
        else
            _safeBoxSearchTime = 0;
        if (_safeBoxSearchTime >= safeBoxDeleteTime)
        {
            safeBox.HalveMoney();
            Destroy();
        }
    }
    
    protected override void Destroy()
    {
        base.Destroy();
        isDestroy = true;
        GameManager.Instance.safeBox.isInteractable = true;
    }
}
