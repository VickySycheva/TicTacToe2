using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int Index => index;
    public Figure CellObject { get; private set; }
    
    [SerializeField] private Vector2Int index;

    public void Init()
    {
        ClearCell();
    }

    public bool CanPlace(Figure selectedFigure) => CellObject == null || CellObject.Power < selectedFigure.Power;

    public void SetCellObject(Figure selectedObj)
    {
        ClearCell();
        CellObject = selectedObj;
        CellObject.transform.parent = transform;
        CellObject.transform.localPosition = Vector3.zero;
        CellObject.DestroyColl();
    }

    public void ClearCell()
    {
        if (CellObject != null)
        {
            Destroy(CellObject.gameObject);
            CellObject = null;
        }
    }

}
