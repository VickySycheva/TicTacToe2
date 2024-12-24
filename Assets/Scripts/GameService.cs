using System;
using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private InputManager inputManager;

    [Header("Prefabs"), Space]
    [SerializeField] private PlayingField playingFieldPrefab;
    [SerializeField] private FiguresOfPlayer crossPlayerPrefab;
    [SerializeField] private FiguresOfPlayer naughtPlayerPrefab;
    
    bool isWin;
    PlayerType currentPlayer;
    PlayingField playingField;

    List<Figure> crossesFigures = new List<Figure>();
    List<Figure> naughtsFigures = new List<Figure>();

    List<Cell> matchingCells = new List<Cell> ();
    Cell startCell = null;
    int startInd = 0;

    Figure selectedFigure;

    void Start()
    {
        uiManager.Init(StartGame, RestartGame);
        inputManager.ActionOnCellClick += OnCellClick;
        inputManager.ActionOnPlayerFigureClick += OnFigureClick;
    }

    void StartGame()
    {
        uiManager.CloseStartScreen();
        
        playingField = Instantiate(playingFieldPrefab);
        playingField.Init();
        
        InstFigures();
        UpdateUiAndInputManager();
    }
    
    void RestartGame()
    {
        uiManager.CloseEndScreen();

        foreach (Cell cell in playingField.CellsOnField)
        {
            cell.ClearCell();
        }

        foreach (Figure figure in crossesFigures)
        {
            Destroy(figure.gameObject);
        }
        crossesFigures = new List<Figure> ();

        foreach (Figure figure in naughtsFigures)
        {
            Destroy(figure.gameObject);
        }
        naughtsFigures = new List<Figure> ();

        InstFigures();
        UpdateUiAndInputManager();
    
    }

    void InstFigures()
    {
        FiguresOfPlayer crossPlayer = Instantiate(crossPlayerPrefab);
        crossPlayer.Init(PlayerType.Cross);
        crossesFigures = crossPlayer.GetFigures();

        FiguresOfPlayer naughtPlayer = Instantiate(naughtPlayerPrefab);
        naughtPlayer.Init(PlayerType.Naught);
        naughtsFigures = naughtPlayer.GetFigures();
    }

    void UpdateUiAndInputManager()
    {
        currentPlayer = PlayerType.Cross;
        isWin = false;
        inputManager.EnableInput(true);
        uiManager.UpdateGameText(currentPlayer);
        uiManager.GameScreenSetActive(true);
    }

    void OnCellClick(Cell cell)
    {
        if (selectedFigure == null) return;

        if (!cell.CanPlace(selectedFigure)) return;

        cell.SetCellObject(selectedFigure);

        if(selectedFigure.PlayerType == PlayerType.Cross)
            crossesFigures.Remove(selectedFigure);
        else
            naughtsFigures.Remove(selectedFigure);

        Check(currentPlayer);
        selectedFigure = null;
        ChangePlayer();
    }

    void OnFigureClick (Figure playerFigure)
    {
        if (currentPlayer != playerFigure.PlayerType) return;

        if (selectedFigure == null)
        {
            selectedFigure = playerFigure;
            selectedFigure.OnFigureClick(true);
        }    
        else 
        {
            selectedFigure.OnFigureClick(false);
            selectedFigure = playerFigure;
            selectedFigure.OnFigureClick(true);
        }
    }

    void ChangePlayer()
    {
        currentPlayer = currentPlayer == PlayerType.Cross ? PlayerType.Naught : PlayerType.Cross;
        uiManager.UpdateGameText(currentPlayer);
    }

    void Check(PlayerType currentPlayer)
    {
        CheckHorizontal(currentPlayer);
        if(!isWin)
           CheckVertical(currentPlayer);
        if(!isWin)
           CheckDiagonal(currentPlayer);

        if(isWin || !CheckFreeCell())
        {
           EndGame(isWin, currentPlayer);
        }
    }

    void CheckHorizontal(PlayerType currentPlayer)
    {
        for (int i = 0; i < 3; i++)
        {
            matchingCells.Clear();
            startCell = null;

            for(int j = 0; j < 3; j++)
            {
                if (playingField.CellsOnField[i,j].CellObject != null && playingField.CellsOnField[i,j].CellObject.PlayerType == currentPlayer)
                {
                    if(startCell == null || MathF.Abs(startInd - j) == 1)
                    {
                        startCell = playingField.CellsOnField[i,j];
                        startInd = j;
                        matchingCells.Add(startCell);
                    }
                    else
                    {
                        matchingCells.Clear();
                        startCell = null;
                    }
                }
                if(matchingCells.Count == 3)
                {
                    isWin = true;
                    return;
                }
            }
        }
    }

    void CheckVertical(PlayerType currentPlayer)
    {
        for (int j = 0; j < 3; j++)
        {
            matchingCells.Clear();
            startCell = null;

            for(int i = 0; i < 3; i++)
            {
                if (playingField.CellsOnField[i,j].CellObject != null && playingField.CellsOnField[i,j].CellObject.PlayerType == currentPlayer)
                {
                    if(startCell == null || MathF.Abs(startInd - i) == 1)
                    {
                        startCell = playingField.CellsOnField[i,j];
                        startInd = i;
                        matchingCells.Add(startCell);
                    }
                    else
                    {
                        matchingCells.Clear();
                        startCell = null;
                    }
                }
                if(matchingCells.Count == 3)
                {
                    isWin = true;
                    return;
                }
            }
        }
    }
    
    void CheckDiagonal(PlayerType currentPlayer)
    {
        matchingCells.Clear();
        for (int i = 0; i < 3; i++)
        {
            if (playingField.CellsOnField[i,i].CellObject != null && playingField.CellsOnField[i,i].CellObject.PlayerType == currentPlayer)
            {
                matchingCells.Add(playingField.CellsOnField[i,i]);
            }
            if(matchingCells.Count == 3)
            {
                isWin = true;
                return;
            }
        }
        matchingCells.Clear();
        for (int i = 0; i < 3; i++)
        {
            if (playingField.CellsOnField[i,2-i].CellObject != null && playingField.CellsOnField[i,2-i].CellObject.PlayerType == currentPlayer)
            {
                matchingCells.Add(playingField.CellsOnField[i,2-i]);
            }
            if(matchingCells.Count == 3)
            {
                isWin = true;
                return;
            }
        } 
    }

    bool CheckFreeCell()
    {
        foreach (Figure figure in crossesFigures)
        {
            foreach (Cell cell in playingField.CellsOnField)
            {
                if (cell.CanPlace(figure)) return true;
            }
        }

        foreach (Figure figure in naughtsFigures)
        {
            foreach (Cell cell in playingField.CellsOnField)
            {
                if (cell.CanPlace(figure)) return true;
            }
        }
        return false;
    }

    void EndGame(bool isWin, PlayerType player)
    {
        inputManager.EnableInput(false);
        uiManager.GameScreenSetActive(false);
        uiManager.ShowEndScreen(isWin, player);
    }
}
