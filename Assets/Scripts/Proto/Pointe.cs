using System;
using UnityEngine;

public class Pointe : MonoBehaviour
{
    public PlayerControllerProto controller;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pointe"))
        {
            if (controller.CurrentSpeed < other.GetComponent<Pointe>().controller.CurrentSpeed)
            {
                Destroy(transform.parent.gameObject);
            }
            
        }
        else
        {
            Destroy(other.transform.gameObject);
        }
        
    }
}
