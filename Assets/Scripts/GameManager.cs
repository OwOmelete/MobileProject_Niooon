using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int RoundToWin;
    private int CurrentP1Score;
    private int CurrentP2Score;
    private int CurrentRound;

    public MenuManager menuManager;
    [SerializeField] private TMP_Text UIScore;
    [SerializeField] private Image[] UIScoreNotches;
    [SerializeField] private TMP_Text UIRound;

    public static GameManager INSTANCE;

    public event Action OnGameStart;

    public enum GameState
    {
        MainMenu,
        PreparingGame,
        InGame
    }

    public GameState currentState;


    private void OnEnable()
    {
        MenuManager.OnRoundTransitionEnd += EndRound;
    }

    private void OnDisable()
    {
        MenuManager.OnRoundTransitionEnd -= EndRound;
    }

    public void SwitchGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                
                ResetGame();
                UpdateScore();
                
                break;
            case GameState.PreparingGame:
                break;
            case GameState.InGame:
                
                ResetGame();
                
                OnGameStart?.Invoke();
                
                
                break;
        }

        currentState = newState;
    }

    public void MainToGame()
    {
        SwitchGameState(GameManager.GameState.InGame);
        menuManager.MainToGame();
    }

    public void GameToMain()
    {
        SwitchGameState(GameManager.GameState.MainMenu);
        menuManager.GameToMain();
    }
    
    
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
        }
        INSTANCE = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        UpdateScore();
        ResetScoreUi();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UpdateScore();
    }

    public void ResetGame()
    {
        CurrentP1Score = 0;
        CurrentP2Score = 0;
        CurrentRound = 0;
        ResetScoreUi();
        UIRound.text = "Round " + (CurrentRound+1);
    }

    public void AddScore(int player)
    {
        if (player == 1)
        {
            CurrentP1Score++;
        }
        else if (player == 2)
        {
            CurrentP2Score++;
        }
        AddScoreToUi(player);
        UpdateScore();
    }

    private void AddScoreToUi(int player)
    {
        UIRound.text = "Round " + (CurrentRound+1);
        if (player == 1)
        {
            UIScoreNotches[CurrentRound-1].color = Color.blue;
        }
        else
        {
            UIScoreNotches[CurrentRound-1].color = Color.red;
        }
    }

    private void ResetScoreUi()
    {
        foreach (Image img in UIScoreNotches)
        {
            img.color = Color.gray;
        }
    }
    
    public void EndRound()
    {
        if (CurrentP1Score >= RoundToWin || CurrentP2Score >= RoundToWin)
        {
            EndGame();
            
        }
        CurrentRound += 1;
    }

    private void EndGame()
    {
        ResetGame();
        GameToMain();
    }

    private void UpdateScore()
    {
        UIScore.text = CurrentP1Score + " - " + CurrentP2Score;
    }
    
}
