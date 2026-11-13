
using System;
using System.Collections;

using TMPro;
using Unity.Mathematics;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movements")]
    private bool isOnWall = false;
    [SerializeField] private Stick slidingStick;

    [SerializeField] private float WalkSpeed;

    [Header("Other references")] [SerializeField]
   
    [HideInInspector] public Vector2 CurrentDirection;
    [HideInInspector] public float CurrentSpeed;

    private Vector2 StickInputs;
    [SerializeField] private TMP_Text speedDisplay;
    [SerializeField] private WalkController walkController;
    private float maxSpeedPercent;

    [SerializeField] private LayerMask collisionMask;

    [SerializeField] private PlayerVisuals visuals;

    public Player player;

    [SerializeField] private bool AutoMove = false;
    [SerializeField] private Button Accel;
    [SerializeField] private GameObject Arrow;

    

    bool c = false;
    private bool isWallStick = false;

    private void DisplayHp()
    {
        //hpDisplay.text = (Mathf.Round(hp * 10) * 0.1f).ToString();
    }

    private void OnEnable()
    {
        MenuManager.OnRoundTransitionEnd += RespawnPlayer;
    }

    private void OnDisable()
    {
        MenuManager.OnRoundTransitionEnd -= RespawnPlayer;
    }

    public void AutoMoveTrue()
    {
        AutoMove = true;
    }

    public void AutoMoveFalse()
    {
        AutoMove = false;
    }

    public void changeStick()
    {
        if (c)
        {
            slidingStick.transform.parent.parent.gameObject.SetActive(true);
            c = false;
        }
        else
        {
            slidingStick.transform.parent.parent.gameObject.SetActive(false);
            c = true;
        }
    }


    private void Start()
    {
        player.canMove = true;
        DisplayHp();
        CurrentDirection = Vector2.up;

        walkController.stick = slidingStick;
        walkController.collisionMask = collisionMask;
    }

    
    
    private void FixedUpdate()
    {
        if (!player.canMove)
        {
            return;
        }

        if (player.CurrentState == Player.PlayerState.OnHorse)
        {
            StickInputs = slidingStick.Direction.normalized;
        }
        
        
        maxSpeedPercent = CurrentSpeed / player.maxSpeed;

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
        if (StickInputs != Vector2.zero)
        {
            Arrow.SetActive(true);
            Arrow.transform.rotation = Quaternion.Euler(0,0, Mathf.Atan2(StickInputs.y, StickInputs.x) * Mathf.Rad2Deg - 90);
        }
        else
        {
            Arrow.SetActive(false);
        }
    }

    private void RespawnPlayer()
    {
        
        CurrentSpeed = 0;
        AutoMove = false;
        //player.ResetPlayer();
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
        if (player.CurrentState == Player.PlayerState.OnFoot)
        {
            CurrentSpeed -= player.maxDeceleration * player.decelerationCurve.Evaluate(maxSpeedPercent) * Time.fixedDeltaTime;
        }
        else if (StickInputs == Vector2.zero && (AutoMove ))
        {
            CurrentSpeed += player.acceleration * player.accelerationCurve.Evaluate(maxSpeedPercent) *
                            Time.fixedDeltaTime;
        }
        else if (isWallStick && AutoMove)
        {
            CurrentSpeed += player.acceleration * player.accelerationCurve.Evaluate(maxSpeedPercent) *
                            Time.fixedDeltaTime;
        }
        else if (AutoMove)
        {
            CurrentSpeed += (Vector2.Dot(StickInputs, CurrentDirection) - player.forwardThreshold) * player.acceleration * player.accelerationCurve.Evaluate(maxSpeedPercent) / ((player.forwardThreshold - 1) * -1) * Time.fixedDeltaTime;
        }
        else 
        {
            CurrentSpeed -= player.maxDeceleration * player.decelerationCurve.Evaluate(maxSpeedPercent) * Time.fixedDeltaTime;
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
            float dotInput = Vector2.Dot(StickInputs, Vector2.Perpendicular(leftHit.normal));
            float angle = Vector2.Angle(Vector2.Perpendicular(leftHit.normal), -leftHit.normal);
            
            if (Mathf.Acos((dotInput)) * Mathf.Rad2Deg > player.maxAngleWhileSliding && !IsBetween(StickInputs, -leftHit.normal, Vector2.Perpendicular(leftHit.normal)))
            {
                if (dot < -player.slideThreshold && !isOnWall)
                {
                    CurrentSpeed *= Mathf.Clamp((1 + dot)*3, 0, 1);
                }
                if (true)
                {
                    Vector2 newDir = CurrentDirection-leftHit.normal * dot;
                
                    CurrentDirection = Vector2.Lerp(CurrentDirection, newDir, 0.3f).normalized;
                }
                isWallStick = false;
            }
            else
            {
                CurrentDirection = Vector2.Perpendicular(leftHit.normal).normalized;
                isWallStick = true;
            }
            isOnWall = true;
        }
        else if (rightHit)
        {
            dot = Vector2.Dot(CurrentDirection, rightHit.normal);
            float dotInput = Vector2.Dot(StickInputs, -Vector2.Perpendicular(rightHit.normal));
            
            
            
            if (Mathf.Acos((dotInput)) * Mathf.Rad2Deg > player.maxAngleWhileSliding && !IsBetween(StickInputs, -rightHit.normal, -Vector2.Perpendicular(rightHit.normal)))
            {
                if (dot < -player.slideThreshold && !isOnWall)
                {
                    CurrentSpeed *= Mathf.Clamp((1 + dot)*3, 0, 1);
                }
                
                if (true)
                {
                    Vector2 newDir = CurrentDirection-rightHit.normal * dot;
                    CurrentDirection = Vector2.Lerp(CurrentDirection, newDir, 0.3f).normalized;
                }
                isWallStick = false;
                
            }
            else
            {
                CurrentDirection = -Vector2.Perpendicular(rightHit.normal).normalized;
                isWallStick = true;
            }
            
            isOnWall = true;
        }
        else
        {
            isOnWall = false;
            isWallStick = false;
        }
        
    }

    bool IsBetween(Vector2 target, Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);

        float angleA = Vector2.Angle(vectorA, target);
        float angleB = Vector2.Angle(vectorB, target);


        return angle > angleA && angle > angleB;
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
