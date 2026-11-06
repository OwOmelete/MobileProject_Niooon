using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private Player Player1;
    [SerializeField] private Player Player2;


    private void OnEnable()
    {
        Player.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath(int n)
    {
        if (n == 1)
        {
            GameManager.INSTANCE.AddScore(2);
        }
        else if (n == 2)
        {
            GameManager.INSTANCE.AddScore(1);
        }

        //Player1.canMove = false;
        //Player2.canMove = false;
        
        StartCoroutine(NewRoundDelay());
    }


    IEnumerator NewRoundDelay()
    {
        yield return new WaitForSeconds(3);
        
        StartNewRound();
        
        GameManager.INSTANCE.EndRound();
    }
    
    private void StartNewRound()
    {
        Player1.gameObject.SetActive(true);
        Player2.gameObject.SetActive(true);
        Player1.ResetPlayer();
        Player2.ResetPlayer();
    }
    
}
