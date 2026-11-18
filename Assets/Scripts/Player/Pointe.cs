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
        if (other.CompareTag("Hitbox"))
        {
            Pointe opponentPoint = other.GetComponent<Pointe>();
            if (controller.CurrentSpeed < opponentPoint.controller.CurrentSpeed)
            {
                //controller.player.takeDamage(opponentPoint.CalculateDamage());
                controller.player.takeDamage(1);
            }
            
        }
        else if(other.CompareTag("Hurtbox"))
        {
            if (controller.player.speedState > Player.Speed.low)
            {
                Pointe opponentPoint = other.GetComponentInParent<Pointe>();
                //opponentPoint.controller.player.takeDamage(CalculateDamage());
                opponentPoint.controller.player.takeDamage(1);
            }
        }
        else if (other.CompareTag("OnFoot"))
        {
            WalkController opponent = other.GetComponent<WalkController>();
            if (controller.player.ID == opponent.player.ID) return;
            opponent.player.takeDamage(1);
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
