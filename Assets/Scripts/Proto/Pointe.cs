using System;
using UnityEngine;

public class Pointe : MonoBehaviour
{
    public PlayerControllerProto controller;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (controller.isImmune) return;
        
        if (other.CompareTag("Pointe"))
        {
            Pointe point = other.GetComponent<Pointe>();
            if (controller.CurrentSpeed < point.controller.CurrentSpeed)
            {
                controller.takeDamage(CalculateDamage(controller.CurrentDirection * controller.CurrentSpeed,
                    point.controller.CurrentDirection * point.controller.CurrentSpeed));
            }
            
        }
        else if(other.CompareTag("Player"))
        {
            PlayerControllerProto opponentController = other.GetComponent<PlayerControllerProto>();
            opponentController.takeDamage(CalculateDamage(opponentController.CurrentDirection * opponentController.CurrentSpeed,
                controller.CurrentDirection * controller.CurrentSpeed));
        }
        
    }

    private float CalculateDamage(Vector2 playerVector, Vector2 opponentVector)
    {
        float f = Mathf.Abs (playerVector.magnitude - opponentVector.magnitude);
        return (f * controller.damageMult)+5;
    }

    private bool isTriggeredFromBehind(Collider2D other)
    {
        Vector2 direction = other.transform.position - transform.position;
        return Vector2.Dot(transform.up, direction) <= 0;
    }
}
