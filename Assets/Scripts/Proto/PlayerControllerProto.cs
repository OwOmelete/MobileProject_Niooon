using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerProto : MonoBehaviour
{
    
    [Header("Movements")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed = 0.04f;
    [SerializeField] private float acceleration = 0.03f;
    [SerializeField] private float forwardThreshold = 0.5f;
    [SerializeField] private Joystick joystick;
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
            hpDisplay.text = hp.ToString();
            Destroy(gameObject);
        }
        else
        {
            hpDisplay.text = hp.ToString();
        }
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
        hpDisplay.text = hp.ToString();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        StickInputs = joystick.Direction;
        
    }

    private void FixedUpdate()
    {
        CurrentDirection = Vector2.Lerp(CurrentDirection, StickInputs, turnSpeed ).normalized;
        DirectionToRotation();
        SpeedAmount();
        Thrust();
    }

    private void DirectionToRotation()
    {
        transform.rotation = Quaternion.Euler(0 ,0, -90 + Vector2.Angle(Vector2.right, CurrentDirection) * Mathf.Sign(CurrentDirection.y));
    }

    private void SpeedAmount()
    {
        CurrentSpeed += (Vector2.Dot(StickInputs, CurrentDirection) - forwardThreshold) * acceleration * Time.fixedDeltaTime;
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, maxSpeed);
    }

    private void Thrust()
    {
        transform.position = transform.position + new Vector3(CurrentDirection.x * CurrentSpeed * Time.fixedDeltaTime, CurrentDirection.y * CurrentSpeed * Time.fixedDeltaTime , 0);
    }
}
