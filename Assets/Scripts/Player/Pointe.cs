using System;
using System.Collections;
using UnityEngine;

public class Pointe : MonoBehaviour
{
    public PlayerController controller;
    
    [Header("Health/Damage")]
    

    private bool isImmune = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isImmune) return;
        if (other.CompareTag("Pointe"))
        {
            Pointe opponentPoint = other.GetComponent<Pointe>();
            if (controller.CurrentSpeed < opponentPoint.controller.CurrentSpeed)
            {
                controller.player.takeDamage(opponentPoint.CalculateDamage());
            }
            
        }
        else if(other.CompareTag("Player"))
        {
            Pointe opponentPoint = other.GetComponentInChildren<Pointe>();
            opponentPoint.controller.player.takeDamage(CalculateDamage());
        }
        
    }

    private float CalculateDamage()
    {
        switch (controller.player.speedState)
        {
            case (Player.Speed.stop):
                {
                    return 0;
                }
            case (Player.Speed.low):
                {
                    return controller.player.lowSpeedDamage;
                }
            case (Player.Speed.medium):
                {
                    return controller.player.mediumSpeedDamage;
                }
            case (Player.Speed.high):
                {
                    return controller.player.highSpeedDamage;
                }
        }
        return 0;
    }

    private bool isTriggeredFromBehind(Collider2D other)
    {
        Vector2 direction = other.transform.position - transform.position;
        return Vector2.Dot(transform.up, direction) <= 0;
    }
    
    
    
    
}
