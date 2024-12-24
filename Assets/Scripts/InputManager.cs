using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Cell> ActionOnCellClick;
    public Action<Figure> ActionOnPlayerFigureClick;
    private bool isEnabled;

    public void EnableInput(bool value)
    {
        isEnabled = value;
    }

    void Update()
    {
        if (isEnabled == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out RaycastHit hit, 100))
            {
                if (hit.transform.TryGetComponent(out Cell cell))
                {
                    ActionOnCellClick?.Invoke(cell);
                }
                if (hit.transform.TryGetComponent(out Figure playerFigure))
                {
                    ActionOnPlayerFigureClick?.Invoke(playerFigure);
                }
            }
        }    
    }
}
