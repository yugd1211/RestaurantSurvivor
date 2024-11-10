using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public int maxStorage = 4;
    public float villainDefense = 1;
    public Animator anim;
    public Carryable carriedItem;

    private Coroutine _moveCoroutine;
    private CashierTable _cashierTable;
    private Vector2 _moveDir;
    private bool _isMoving = true;

    private void Reset()
    {
        moveSpeed = 2f;
        anim = GetComponent<Animator>();
        _cashierTable = FindObjectOfType<CashierTable>();
    }

    private void Start()
    {
        _moveCoroutine = StartCoroutine(CoMovePossible());
        _cashierTable = FindObjectOfType<CashierTable>();
    }

    private void Update()
    {
        HandleInput();
        if (_isMoving)
            Move();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 && verticalInput != 0)
        {
            if (horizontalInput > verticalInput)
                verticalInput = 0;
            else
                horizontalInput = 0;
        }

        _moveDir = new Vector2(horizontalInput, verticalInput).normalized;
    }

    private void Move()
    {
        if (GameManager.Instance.isPause || _moveDir == Vector2.zero)
            return;
        TriggerAnimation(_moveDir);
        
        if (!CheckPath(_moveDir))
            return;

        transform.position += new Vector3(_moveDir.x, _moveDir.y, 0);
        _isMoving = false;
    }

    private void TriggerAnimation(Vector2 dir)
    {
        string currentClip = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (dir == Vector2.up && !currentClip.Equals("PlayerUp"))
            anim.SetTrigger("UpTrigger");
        else if (dir == Vector2.down && !currentClip.Equals("PlayerDown"))
            anim.SetTrigger("DownTrigger");
        else if (dir == Vector2.left && !currentClip.Equals("PlayerLeft"))
            anim.SetTrigger("LeftTrigger");
        else if (dir == Vector2.right && !currentClip.Equals("PlayerRight"))
            anim.SetTrigger("RightTrigger");
    }

    private bool CheckPath(Vector2 dir)
    {
        float rayDistance = 1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance,
            LayerMask.GetMask(LayerName.Interactive.ToString(), LayerName.Villain.ToString(), LayerName.Customer.ToString())); 
        return hit.collider == null;
    }

    private IEnumerator CoMovePossible()
    {
        while (true)
        {
            if (_isMoving)
            {
                yield return null;
                continue;
            }
            yield return new WaitForSeconds(1 / moveSpeed);
            _isMoving = true;
        }
    }

    public void SetItem(Carryable item)
    {
        item.maxCount = maxStorage;
        if (carriedItem)
            DestroyItemIfCarried(item);
        else
        {
            carriedItem = item;
            AttachItemToPlayer(item);
        }
    }

    private void DestroyItemIfCarried(Carryable item)
    {
        if (item)
            DestroyImmediate(item.gameObject);
    }

    private void AttachItemToPlayer(Carryable item)
    {
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector3(0, -0.1f, 0);
    }
}
