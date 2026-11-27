using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PortalManager : MonoBehaviour
{
    [SerializeField] private List<PortalPair> PortalPositionsList;
    private List<PortalPair> RemainingPortalPairs = new List<PortalPair>();
    [SerializeField] private int[] PortalNumberByRound;
    


    private void OnEnable()
    {
        MenuManager.OnRoundTransitionEnd += OnRoundStart;
    }

    private void OnDisable()
    {
        MenuManager.OnRoundTransitionEnd -= OnRoundStart;
    }

    private void OnRoundStart()
    {
        Debug.Log(GameManager.INSTANCE.CurrentRound);
        ResetPortals();
        SpawnPortals();
    }
    
    private void SpawnPortals()
    {
        
        for (int i = 0; i < Mathf.Min(PortalNumberByRound[GameManager.INSTANCE.CurrentRound - 1], PortalPositionsList.Count); i++)
        {
            int r = Random.Range(0, RemainingPortalPairs.Count);
            
            Debug.Log(RemainingPortalPairs.Count);
            Debug.Log(r);
            
            RemainingPortalPairs[r].portal1.gameObject.SetActive(true);
            RemainingPortalPairs[r].portal2.gameObject.SetActive(true);
            
            RemainingPortalPairs.RemoveAt(r);
            
        }
        
        
    }

    private void ResetPortals()
    {
        RemainingPortalPairs.Clear();
        for(int i = 0; i < PortalPositionsList.Count; i++)
        {
            PortalPositionsList[i].portal1.gameObject.SetActive(false);
            PortalPositionsList[i].portal2.gameObject.SetActive(false);
            
            RemainingPortalPairs.Add(PortalPositionsList[i]);
        }

        
        
    }
    
}

