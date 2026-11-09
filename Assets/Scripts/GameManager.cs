using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
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
    [SerializeField] private GameObject WinnerUI;

    
    
    public static GameManager INSTANCE;

    private bool hasWinner;
    public int Winner;
    
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
        MenuManager.OnRoundTransitionEnd += EndRoundTransition;
        RoundManager.OnRoundEnd += EndRound;
    }

    private void OnDisable()
    {
        MenuManager.OnRoundTransitionEnd -= EndRoundTransition;
        RoundManager.OnRoundEnd -= EndRound;
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
        DebugManager.instance.enableRuntimeUI = false;
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
        CurrentRound = 1;
        hasWinner = false;
        WinnerUI.SetActive(false);
        ResetScoreUi();
        UIRound.text = "Round " + (CurrentRound);
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
        CurrentRound += 1;
        UIRound.text = "Round " + (CurrentRound);
    }

    private void AddScoreToUi(int player)
    {
        
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
        if (CurrentP1Score >= RoundToWin)
        {
            hasWinner = true;
            WinnerUI.SetActive(true);
            WinnerUI.transform.rotation = Quaternion.Euler(0,0,180);
        }

        if (CurrentP2Score >= RoundToWin)
        {
            hasWinner = true;
            WinnerUI.SetActive(true);
            WinnerUI.transform.rotation = Quaternion.identity;
        }
    }

    public void EndRoundTransition()
    {
        if (hasWinner)
        {
            EndGame();
        }
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
