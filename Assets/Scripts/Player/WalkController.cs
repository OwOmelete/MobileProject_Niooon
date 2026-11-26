using System;
using System.Collections;
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
    private SpriteRenderer sr;
    private Color baseColor;

    private bool isLaunched;

    private float lastEjectTime;
    [SerializeField] private float DelayForMount;

    private Vector2 Input;

    private Vector2 DashDir;
    private bool isDashing;
    [SerializeField] private float DashCooldown;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashSpeed;
    [SerializeField, Range(0, 1)] private float BounceCoefficient;
    [SerializeField] private float MaxDeceleration;
    [SerializeField] private AnimationCurve DecelerationCurve;
    [SerializeField] private float LaunchSpeedToStop;
    private float lastButtonPress;
    [SerializeField] private float TimeForDashInput;
    private float LastDashTime = -1000;

    private Coroutine dmgCoroutine;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    IEnumerator damageAnim()
    {
        while(player.isImmune)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            sr.color = baseColor;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void FixedUpdate()
    {
        if(CurrentSpeed <= LaunchSpeedToStop)
        {
            player.isImmune = false;
            isLaunched = false;
            CurrentSpeed = 0;
        }

        if (isLaunched)
        {
            CurrentSpeed -= MaxDeceleration * Time.fixedDeltaTime;
            Debug.Log(CurrentSpeed);
            checkBounce();
            FinalMovement();
            return;
        }

        Input = stick.Direction.normalized;

        if(isDashing && Time.time > LastDashTime + DashDuration)
        {
            if (player.isImmune)
            {
                player.isImmune = false;
            }
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
        if(Time.time > LastDashTime + DashCooldown && Time.time < lastButtonPress + TimeForDashInput)
        {
            player.isImmune = true;
            isDashing = true;
            LastDashTime = Time.time;
            DashDir = CurrentDirection;
            StopCoroutine(dmgCoroutine);
            dmgCoroutine = StartCoroutine("damageAnim");
        }
        lastButtonPress = Time.time;
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

    private void checkBounce()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)CurrentDirection* collider.radius * transform.localScale.x * 1.1f, CurrentDirection, CurrentSpeed * Time.fixedDeltaTime, collisionMask);
        if (hit)
        {
            CurrentDirection = Vector2.Reflect(CurrentDirection, hit.normal);
            CurrentSpeed *= BounceCoefficient;
        }
    }


    private void FinalMovement()
    {
        Vector3 Displacement;
        if (isDashing)
        {
             Displacement = (Vector3)(DashDir * CurrentSpeed * Time.fixedDeltaTime * DashSpeed);
        }
        else
        {
            Displacement = (Vector3)(CurrentDirection * CurrentSpeed * Time.fixedDeltaTime);
        }
        
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

    public void PlayerLaunched(Vector2 dir, float speed)
    {
        lastEjectTime = Time.time;
        player.isImmune = true;

        isLaunched = true;
        CurrentDirection = dir;
        CurrentSpeed = speed;

        StopCoroutine(dmgCoroutine);
        dmgCoroutine = StartCoroutine("damageAnim");

    }
}
