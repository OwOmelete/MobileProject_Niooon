using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int RoundToWin;
    private int CurrentP1Score;
    private int CurrentP2Score;

    [SerializeField] private TMP_Text UIScore;
    
    public static GameManager INSTANCE;
    
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
        }
        INSTANCE = this;
        DontDestroyOnLoad(this);
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
        UpdateScore();
    }
    
    public void EndRound()
    {
        if (CurrentP1Score >= RoundToWin || CurrentP2Score >= RoundToWin)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("partie terminée");
        
        ResetGame();
    }

    private void UpdateScore()
    {
        UIScore.text = CurrentP1Score + " - " + CurrentP2Score;
    }
    
}
