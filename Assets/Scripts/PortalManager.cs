using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PortalManager : MonoBehaviour
{
    [SerializeField] private List<PortalPair> PortalPositionsList;
    private List<PortalPair> RemainingPortalPairs = new List<PortalPair>();
    [SerializeField] private int[] PortalNumberByRound;

    [SerializeField] private float TimeBetweenSpawns;
    private float LastPortalSpawn;
    private float TotalPortal;

    private void OnEnable()
    {
        MenuManager.OnRoundTransitionEnd += OnRoundStart;
        Portal.OnPortalDisappear += OnPortalDisappear;
    }

    private void OnDisable()
    {
        MenuManager.OnRoundTransitionEnd -= OnRoundStart;
        Portal.OnPortalDisappear -= OnPortalDisappear;
    }

    private void OnPortalDisappear()
    {
        if (TotalPortal == PortalNumberByRound[GameManager.INSTANCE.CurrentRound - 1])
        {
            LastPortalSpawn = Time.time;
        }

        TotalPortal -= 1;
    }

    private void FixedUpdate()
    {
        if (GameManager.INSTANCE.CurrentRound == 0) return;
        if (TotalPortal < PortalNumberByRound[GameManager.INSTANCE.CurrentRound - 1] && Time.time > LastPortalSpawn + TimeBetweenSpawns)
        {
            SpawnOnePortal();
            LastPortalSpawn = Time.time;
        }
    }


    private void OnRoundStart()
    {
        ResetPortals();
        
    }

    private void SpawnOnePortal()
    {
        bool check = false;
        int r = 0;
        
        Debug.Log(PortalPositionsList.Count);
        while (!check)
        {
            r = Random.Range(0, PortalPositionsList.Count);
            Debug.Log(r);
            if (!PortalPositionsList[r].portal1.isActive)
            {
                check = true;
            }
        }
        Debug.Log("hihi");
        PortalPositionsList[r].portal1.gameObject.SetActive(true);
        PortalPositionsList[r].portal1.LightPortal();
        PortalPositionsList[r].portal2.gameObject.SetActive(true);
        PortalPositionsList[r].portal2.LightPortal();

        TotalPortal += 1;
    }
    
    private void SpawnPortals()
    {
        for (int i = 0; i < Mathf.Min(PortalNumberByRound[GameManager.INSTANCE.CurrentRound - 1], PortalPositionsList.Count); i++)
        {
            SpawnOnePortal();
        }
    }

    private void ResetPortals()
    {
        for(int i = 0; i < PortalPositionsList.Count; i++)
        {
            PortalPositionsList[i].portal1.ResetPortals();
            PortalPositionsList[i].portal2.ResetPortals();
        }

        TotalPortal = 0;

        LastPortalSpawn = Time.time;
    }
    
}

