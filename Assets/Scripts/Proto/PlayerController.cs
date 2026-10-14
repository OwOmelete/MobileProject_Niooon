
using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movements")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed = 0.04f;

    [SerializeField] private AnimationCurve rotationCurve;

    [SerializeField] private float acceleration = 0.03f;

    [SerializeField] private AnimationCurve accelerationCurve;

    [SerializeField] private float forwardThreshold = 0.5f;
    [SerializeField] private float slideThreshold;
    [SerializeField] private Joystick joystick;
    [HideInInspector] public bool isImmune;
    [SerializeField, Range(0, 1)] private float lowSpeedThreshold;
    [SerializeField, Range(0, 1)] private float mediumSpeedThreshold;
    [SerializeField, Range(0, 1)] private float highSpeedThreshold;
    private bool isOnWall = false;


    private enum Speed
    {
        stop,
        low,
        medium,
        high
    }

    private Speed speedState;
    


    [HideInInspector] public Gamepad g;
    [Header("Other references")] [SerializeField]
    private Animator trailBehaviour;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject dust;
    [SerializeField] private GameObject stars;
    
    private SpriteRenderer sr;
    private Vector2 StickInputs;
    [HideInInspector] public Vector2 CurrentDirection;
    [HideInInspector] public float CurrentSpeed;

    [SerializeField] private TMP_Text hpDisplay;
    private float maxSpeedPercent;

    [SerializeField] private LayerMask collisionMask;

    

    private void DisplayHp()
    {
        //hpDisplay.text = (Mathf.Round(hp * 10) * 0.1f).ToString();
    }



    private void Start()
    {
        DisplayHp();
        //sr = GetComponent<SpriteRenderer>();

        stars.SetActive(false);
        dust.SetActive(false);

        CurrentDirection = Vector2.up;
    }

    private void FixedUpdate()
    {
        maxSpeedPercent = CurrentSpeed / maxSpeed;

        StickInputs = joystick.Direction;
        CurrentDirection = Vector2.Lerp(CurrentDirection, StickInputs, turnSpeed * rotationCurve.Evaluate(maxSpeedPercent)).normalized;

        SpeedAmount();

        HandleSlide();

        DirectionToRotation();

        hpDisplay.text = CurrentSpeed.ToString();

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
        CurrentSpeed += (Vector2.Dot(StickInputs, CurrentDirection) - forwardThreshold) * acceleration * accelerationCurve.Evaluate(maxSpeedPercent) / ((forwardThreshold-1)*-1) * Time.fixedDeltaTime;
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, maxSpeed);
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
                if (dot < -slideThreshold && !isOnWall)
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
                if (dot < -slideThreshold && !isOnWall)
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
        float speedPercent = CurrentSpeed / maxSpeed;

        if (speedPercent > highSpeedThreshold)
        {
            SwitchSpeedState(Speed.high);
        }
        else if (speedPercent > mediumSpeedThreshold)
        {
            SwitchSpeedState(Speed.medium);
        }
        else if (speedPercent > 0.01f)
        {
            SwitchSpeedState(Speed.low);
        }
        else
        {
            SwitchSpeedState(Speed.stop);
        }
    }


    private void SwitchSpeedState(Speed newState)
    {
        //if (speedState == newState) return;
        ResetAnimBools();

        switch (speedState)
        {
            case Speed.stop:
                trailBehaviour.SetBool("Stop", true);
                dust.SetActive(false);
                stars.SetActive(false);
                break;
            case Speed.low:
                trailBehaviour.SetBool("Low", true);
                dust.SetActive(false);
                stars.SetActive(false);
                break;
            case Speed.medium:
                trailBehaviour.SetBool("Mid", true);
                dust.SetActive(true);
                stars.SetActive(false);
                break;
            case Speed.high:
                trailBehaviour.SetBool("High", true);
                dust.SetActive(true);
                stars.SetActive(true);
                break;
        }

        speedState = newState;
    }

    private void ResetAnimBools()
    {
        foreach (var param in trailBehaviour.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                trailBehaviour.SetBool(param.name, false);
            }
        }
    }

    #endregion


}
