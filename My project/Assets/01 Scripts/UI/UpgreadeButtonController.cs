using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridListNode<T>
{
    public T Value;
    public GridListNode<T> Top;
    public GridListNode<T> Bottom;
    public GridListNode<T> Left;
    public GridListNode<T> Right;

    public GridListNode(T value)
    {
        Value = value;
    }
}

public class GridList<T>
{
    private GridListNode<T>[,] gridNodes;
    public GridListNode<T> currentNode { get; private set; }

    public GridList(List<T> row1Elements, List<T> row2Elements)
    {
        int rowCount = 2;
        int colCount = Mathf.Max(row1Elements.Count, row2Elements.Count);

        gridNodes = new GridListNode<T>[rowCount, colCount];

        for (int i = 0; i < row1Elements.Count; i++)
        {
            gridNodes[0, i] = new GridListNode<T>(row1Elements[i]);
        }

        for (int i = 0; i < row2Elements.Count; i++)
        {
            gridNodes[1, i] = new GridListNode<T>(row2Elements[i]);
        }

        SetNodeConnections(rowCount, colCount);

        currentNode = gridNodes[0, 0];
    }

    private void SetNodeConnections(int rowCount, int colCount)
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                if (gridNodes[row, col] == null)
                    continue;
                GridListNode<T> currNode = gridNodes[row, col];
                GridListNode<T> leftNode = col > 0 ? gridNodes[row, col - 1] : null;
                GridListNode<T> topNode = row > 0 ? gridNodes[row - 1, col] : null;
                
                if (leftNode != null)
                {
                    currNode.Left = leftNode;
                    leftNode.Right = currNode;
                }
                if (topNode != null)
                {
                    currNode.Top = topNode;
                    topNode.Bottom = currNode;
                }
            }
        }
    }


    public void Move(Vector2 dir)
    {
        if (dir == Vector2.up && currentNode.Top != null)
            currentNode = currentNode.Top;
        else if (dir == Vector2.down && currentNode.Bottom != null)
            currentNode = currentNode.Bottom;
        else if (dir == Vector2.left && currentNode.Left != null)
            currentNode = currentNode.Left;
        else if (dir == Vector2.right && currentNode.Right != null)
            currentNode = currentNode.Right;
    }
}
public class UpgreadeButtonController : MonoBehaviour
{
    public GameObject cursor;
    private GridList<UpgradeButton> _gridList;

    private void Start()
    {
        List<UpgradeButton> playerUpgradeButtons = GameObject.Find("PlayerUpgrade").GetComponentsInChildren<UpgradeButton>().ToList();
        List<UpgradeButton> opjectUpgradeButtons = GameObject.Find("ObjectUpgrade").GetComponentsInChildren<UpgradeButton>().ToList();
        
        
        _gridList = new GridList<UpgradeButton>(playerUpgradeButtons, opjectUpgradeButtons);
    }

    private void Update()
    {
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
            _gridList.Move(Vector2.up);
        else if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
            _gridList.Move(Vector2.down);
        else if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
            _gridList.Move(Vector2.left);
        else if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
            _gridList.Move(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.Return))
            _gridList.currentNode.Value.Upgrade();
        
        cursor.transform.position = _gridList.currentNode.Value.transform.position;
    }
}
