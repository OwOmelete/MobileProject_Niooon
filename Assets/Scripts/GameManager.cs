using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController[] playerList;

    [SerializeField] private int RoundToWin;
    private int CurrentP1Score;
    private int CurrentP2Score;

    public static GameManager INSTANCE;
    
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
        }
        INSTANCE = this;
        DontDestroyOnLoad(this);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        
    }
    
}
