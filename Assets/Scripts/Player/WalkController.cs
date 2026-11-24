using System;
using UnityEngine;

public class WalkController : MonoBehaviour
{
    [SerializeField] private float WalkSpeed;
    public Player player;
    
    public Stick stick;
    public LayerMask collisionMask;

    private Vector2 CurrentDirection;
    private float CurrentSpeed;
    [SerializeField]private CircleCollider2D collider;

    private float lastEjectTime;
    [SerializeField] private float DelayForMount;

    private Vector2 Input;

    private Vector2 DashDir;
    private bool isDashing;
    [SerializeField] private float DashCooldown;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashSpeed;
    private float LastDashTime;

    private void FixedUpdate()
    {
        Input = stick.Direction.normalized;

        if(isDashing && Time.time > LastDashTime + DashDuration)
        {
            isDashing = false;
        }

        CurrentDirection = Input;
        
        CurrentSpeed = WalkSpeed;
        
        FinalMovement();
    }

    private void OnEnable()
    {
        player.button.onClick.AddListener(Dash);
    }

    private void OnDisable()
    {
        player.button.onClick.RemoveListener(Dash);
    }

    private void Dash()
    {
        if(Time.time > LastDashTime + DashCooldown)
        {
            
            isDashing = true;
            LastDashTime = Time.time;
            DashDir = CurrentDirection;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hurtbox") && Time.time > lastEjectTime + DelayForMount)
        {
            Player p = other.transform.parent.parent.parent.GetComponent<Player>();
            if (p.ID == player.ID)
            {
                p.OnHorse();
            }
        }
    }


    private void FinalMovement()
    {
        Vector3 Displacement = (Vector3)(CurrentDirection * CurrentSpeed * Time.fixedDeltaTime);
        if (isDashing) Displacement *= DashSpeed;
        transform.position += Displacement;
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
        lastEjectTime = Time.time;
        //launch


    }
}
