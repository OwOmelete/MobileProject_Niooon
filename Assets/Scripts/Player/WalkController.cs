using System;
using UnityEngine;

public class WalkController : MonoBehaviour
{
    [SerializeField] private float WalkSpeed;
    
    public Stick stick;
    public LayerMask collisionMask;

    private Vector2 CurrentDirection;
    private float CurrentSpeed;
    [SerializeField]private CircleCollider2D collider;

    private Vector2 Input;

    private void FixedUpdate()
    {
        Input = stick.Direction.normalized;

        CurrentDirection = Input;
        
        CurrentSpeed = WalkSpeed;
        
        FinalMovement();
        
    }
    
    private void FinalMovement()
    {
        transform.position += (Vector3)(CurrentDirection * CurrentSpeed * Time.fixedDeltaTime);
    }
    
    /*private bool PositionInterpolation()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)CurrentDirection* collider.radius * transform.localScale.x, CurrentDirection, CurrentSpeed * Time.fixedDeltaTime, collisionMask);

        if (hit)
        {
            transform.position = hit.point ;
            return true;
        }

        return false;
    }*/

    public void PlayerLaunched()
    {
        //launch
        
        
    }
}
