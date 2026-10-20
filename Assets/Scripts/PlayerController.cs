
using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movements")]
    private bool isOnWall = false;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Stick slidingStick;


    [Header("Other references")] [SerializeField]
   
    [HideInInspector] public Vector2 CurrentDirection;
    [HideInInspector] public float CurrentSpeed;

    private Vector2 StickInputs;
    [SerializeField] private TMP_Text speedDisplay;
    private float maxSpeedPercent;

    [SerializeField] private LayerMask collisionMask;

    [SerializeField] private PlayerVisuals visuals;

    public Player player;

    [SerializeField] private bool AutoMove = false;



    private void DisplayHp()
    {
        //hpDisplay.text = (Mathf.Round(hp * 10) * 0.1f).ToString();
    }



    private void Start()
    {
        DisplayHp();
        CurrentDirection = Vector2.up;
    }

    private void FixedUpdate()
    {
        maxSpeedPercent = CurrentSpeed / player.maxSpeed;

        if (joystick.enabled)
        {
            StickInputs = joystick.Direction;
        }
        else
        {
            StickInputs = slidingStick.Direction.normalized;
        }
        CurrentDirection = Vector2.Lerp(CurrentDirection, StickInputs, player.maxRotationSpeed * player.rotationCurve.Evaluate(maxSpeedPercent)).normalized;

        SpeedAmount();

        HandleSlide();

        DirectionToRotation();

        speedDisplay.text = CurrentSpeed.ToString();

        FinalMovement();
    }
    private void Update()
    {
        UpdateTrail();
    }



    private void FinalMovement()
    {
        if (!PositionInterpolation())
        {
            transform.position += (Vector3)(CurrentDirection * CurrentSpeed * Time.fixedDeltaTime);
        }
    }
    
    private bool PositionInterpolation()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, CurrentDirection, CurrentSpeed * Time.fixedDeltaTime, collisionMask);

        if (hit)
        {
            transform.position = hit.point;
            return true;
        }

        return false;
    }
    
    

    private void DirectionToRotation()
    {
        //transform.rotation = Quaternion.Euler(0 ,0, -90 + Vector2.Angle(Vector2.right, CurrentDirection) * Mathf.Sign(CurrentDirection.y));

        float angle = Mathf.Atan2(CurrentDirection.y,CurrentDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angle - 90f
        );
    }

    private void SpeedAmount()
    {
        if (StickInputs == Vector2.zero && AutoMove)
        {
            CurrentSpeed += player.acceleration * player.accelerationCurve.Evaluate(maxSpeedPercent) *
                            Time.fixedDeltaTime;
        }
        else
        {
            CurrentSpeed += (Vector2.Dot(StickInputs, CurrentDirection) - player.forwardThreshold) * player.acceleration * player.accelerationCurve.Evaluate(maxSpeedPercent) / ((player.forwardThreshold -1)*-1) * Time.fixedDeltaTime;
        }
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, player.maxSpeed);
    }


    void HandleSlide()
    {
        

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position - transform.up * 0.25f, (transform.up - transform.right * 0.8f), 0.6f, collisionMask);

        RaycastHit2D rightHit = Physics2D.Raycast(transform.position - transform.up * 0.25f, (transform.up + transform.right * 0.8f), 0.6f, collisionMask);

        Debug.DrawRay(transform.position - transform.up * 0.25f, (transform.up - transform.right * 0.8f)*0.6f);


        float dot = 1;
        if (leftHit)
        {
            dot = Vector2.Dot(CurrentDirection, leftHit.normal);
            if (dot < 0)
            {
                if (dot < -player.slideThreshold && !isOnWall)
                {
                    CurrentSpeed *= Mathf.Clamp((1 + dot)*3, 0, 1);
                }
                Vector2 newDir = CurrentDirection-leftHit.normal * dot;
                
                CurrentDirection = Vector2.Lerp(CurrentDirection, newDir, 0.3f).normalized;
            }
            isOnWall = true;
        }
        else if (rightHit)
        {
            dot = Vector2.Dot(CurrentDirection, rightHit.normal);
            if (dot < 0)
            {
                if (dot < -player.slideThreshold && !isOnWall)
                {
                    CurrentSpeed *= Mathf.Clamp((1 + dot) * 3, 0, 1);
                }
                Vector2 newDir = CurrentDirection-rightHit.normal * dot;
                CurrentDirection = Vector2.Lerp(CurrentDirection, newDir, 0.3f).normalized;
            }
            isOnWall = true;
        }
        else
        {
            isOnWall = false;
        }
        
    }

    #region Visuals


    private void UpdateTrail()
    {
        float speedPercent = CurrentSpeed / player.maxSpeed;

        if (speedPercent > player.highSpeedThreshold)
        {
            SwitchSpeedState(Player.Speed.high);
        }
        else if (speedPercent > player.mediumSpeedThreshold)
        {
            SwitchSpeedState(Player.Speed.medium);
        }
        else if (speedPercent > player.lowSpeedThreshold)
        {
            SwitchSpeedState(Player.Speed.low);
        }
        else
        {
            SwitchSpeedState(Player.Speed.stop);
        }
    }


    private void SwitchSpeedState(Player.Speed newState)
    {
        //if (speedState == newState) return;
        visuals.ResetAnimBools();

        switch (player.speedState)
        {
            case Player.Speed.stop:
                visuals.ToStop();
                break;
            case Player.Speed.low:
                visuals.ToLow();
                break;
            case Player.Speed.medium:
                visuals.ToMedium();
                break;
            case Player.Speed.high:
                visuals.ToHigh();
                break;
        }

        player.speedState = newState;
    }


    #endregion


}
