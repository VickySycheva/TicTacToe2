using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("Start Screen"), Space]
    [SerializeField] private Canvas startScreen;
    [SerializeField] private Button startButton;
    
    [Header("End Screen"), Space]
    [SerializeField] private Canvas endScreen;
    [SerializeField] private TMP_Text endText;
    [SerializeField] private Button restartButton;

    [Header("Game Screen"), Space]
    [SerializeField] private Canvas gameScreen;
    [SerializeField] private TMP_Text gameText;

    public void Init(Action onStart, Action onRestart)
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => onStart.Invoke());
        
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => onRestart.Invoke());

        CloseEndScreen();
        GameScreenSetActive(false);
    }   

    public void GameScreenSetActive(bool value)
    {
        gameScreen.gameObject.SetActive(value);
    }

    public void CloseStartScreen()
    {
        startScreen.gameObject.SetActive(false);
    }
    public void CloseEndScreen()
    {
        endScreen.gameObject.SetActive(false);
    }

    public void ShowEndScreen(bool isWin, PlayerType playerType)
    {
        if(isWin)
        {
            endText.text = playerType.ToString() + " is Winner";
        }
        else
        {
            endText.text = "Dead Heat";
        }
        endScreen.gameObject.SetActive(true);
    }

    public void UpdateGameText(PlayerType playerType)
    {
        gameText.text = "Player: " + playerType.ToString();
    }
}