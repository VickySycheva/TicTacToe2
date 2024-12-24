using UnityEngine;

public class PlayingField : MonoBehaviour
{
    [SerializeField] Cell[] cells;

    public Cell[,] CellsOnField { get; private set; }
    
    public void Init()
    {
        CellsOnField = new Cell [3,3];
        foreach (var cell in cells)
        {
            CellsOnField[cell.Index.x, cell.Index.y] = cell;
            CellsOnField[cell.Index.x, cell.Index.y].Init();
        }
    }
}
