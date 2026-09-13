using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerProto : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private float maxSpeed;
    
    private Vector2 StickInputs;
    private Vector2 CurrentDirection;
    private float CurrentSpeed;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        StickInputs = Gamepad.current.leftStick.value.normalized;
        CurrentDirection = Vector2.Lerp(CurrentDirection, StickInputs, 0.008f).normalized;
        Debug.Log(StickInputs);
    }

    private void FixedUpdate()
    {
        DirectionToRotation();
        SpeedAmount();
        Thrust();
    }

    private void DirectionToRotation()
    {
        transform.rotation = Quaternion.Euler(90 ,0, -90 + Vector2.Angle(Vector2.right, CurrentDirection) * Mathf.Sign(CurrentDirection.y));
    }

    private void SpeedAmount()
    {
        CurrentSpeed += (Vector2.Dot(StickInputs, CurrentDirection) - 0.3f) * 0.0005f;
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, maxSpeed);
        
        textField.text = CurrentSpeed.ToString();
    }

    private void Thrust()
    {
        transform.position = transform.position + new Vector3(CurrentDirection.x * CurrentSpeed, 0 , CurrentDirection.y * CurrentSpeed);
    }
}
