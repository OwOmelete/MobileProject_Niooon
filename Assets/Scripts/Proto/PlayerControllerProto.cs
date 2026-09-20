using System;
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

    [HideInInspector] public Gamepad g;
    
    private Vector2 StickInputs;
    private Vector2 CurrentDirection;
    [HideInInspector] public float CurrentSpeed;
    
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
