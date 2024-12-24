using System.Collections.Generic;
using UnityEngine;

public class FiguresOfPlayer : MonoBehaviour
{
    [SerializeField] List<Figure> figures;

    public void Init(PlayerType playerType)
    {
        foreach (var figure in figures)
        {
            figure.Init(playerType);
        }
    }

    public List<Figure> GetFigures()
    {
        return figures;
    }
}
