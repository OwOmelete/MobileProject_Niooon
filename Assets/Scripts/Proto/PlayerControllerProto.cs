using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerProto : MonoBehaviour
{
    
    [Header("Movements")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed = 0.04f;
    [SerializeField] private float acceleration = 0.03f;
    [SerializeField] private float forwardThreshold = 0.5f;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float raysOffset;
    [HideInInspector] public bool isImmune;

    [SerializeField] private float blinkNumberAnim = 4;
    [SerializeField] public float damageMult = 3;
    
    [HideInInspector] public Gamepad g;

    public float hp = 20;

    private SpriteRenderer sr;
    private Vector2 StickInputs;
    [HideInInspector] public Vector2 CurrentDirection;
    [HideInInspector] public float CurrentSpeed;

    [SerializeField] private TMP_Text hpDisplay;

    public void takeDamage(float n)
    {
        if (isImmune) return;
        hp -= n;
        StartCoroutine(damageAnim());
        if (hp < 0)
        {
            hp = 0;
            DisplayHp();
            Destroy(gameObject);
        }
        else
        {
            DisplayHp();
        }
    }

    private void DisplayHp()
    {
        hpDisplay.text = (Mathf.Round(hp * 10) * 0.1f).ToString();
    }

    IEnumerator damageAnim()
    {
        isImmune = true;
        Color baseColor = sr.color;
        for (int i = 0; i < blinkNumberAnim; i++)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            sr.color = baseColor;
            yield return new WaitForSeconds(0.2f);
        }

        isImmune = false;
    }

    private void Start()
    {
        DisplayHp();
        sr = GetComponent<SpriteRenderer>();
        CurrentDirection = Vector2.up;
    }

    private void FixedUpdate()
    {
        StickInputs = joystick.Direction;
        CurrentDirection = Vector2.Lerp(CurrentDirection, StickInputs, turnSpeed ).normalized;

        SpeedAmount();

        HandleSlide();

        DirectionToRotation();

        transform.position += (Vector3)(CurrentDirection * CurrentSpeed * Time.fixedDeltaTime);

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
        CurrentSpeed += (Vector2.Dot(StickInputs, CurrentDirection) - forwardThreshold) * acceleration * Time.fixedDeltaTime;
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, maxSpeed);
    }


    void HandleSlide()
    {
        /*LayerMask mask = LayerMask.GetMask("Wall"); 

        Debug.DrawRay(transform.position - transform.right * raysOffset - transform.up * 0.1f, (transform.up - transform.right * 0.2f)*0.2f);
        Debug.DrawRay(transform.position + transform.right * raysOffset - transform.up * 0.1f, (transform.up + transform.right * 0.2f)*0.2f);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position - transform.right * raysOffset - transform.up * 0.1f, (transform.up - transform.right * 0.2f), 0.2f, mask);

        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + transform.right * raysOffset - transform.up * 0.1f, (transform.up + transform.right * 0.2f), 0.2f, mask);

        if (leftHit)
        {
            Debug.Log("ha");
            //float dot = Vector2.Dot()

            CurrentDirection =
            CurrentDirection = -leftHit.normal;
        }
        else if (rightHit)
        {
            Debug.Log("ho");
            CurrentDirection = -rightHit.normal;
        }
        */

        LayerMask mask = LayerMask.GetMask("Walls");

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position - transform.right * raysOffset - transform.up * 0.1f, (transform.up - transform.right * 0.2f), 0.2f, mask);

        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + transform.right * raysOffset - transform.up * 0.1f, (transform.up + transform.right * 0.2f), 0.2f, mask);



        float dot = 1;
        if (leftHit)
        {
            dot = Vector2.Dot(CurrentDirection, leftHit.normal);
            if (dot < 0)
            {
                CurrentDirection -= leftHit.normal * dot;
                CurrentDirection.Normalize();
            }
        }
        else if (rightHit)
        {
            dot = Vector2.Dot(CurrentDirection, rightHit.normal);
            if (dot < 0)
            {
                CurrentDirection -= rightHit.normal * dot;
                CurrentDirection.Normalize();
            }
        }

    }
}
