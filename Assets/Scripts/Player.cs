using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movements")]
    [HideInInspector] public float maxSpeed;
    [HideInInspector] public float maxRotationSpeed = 0.04f;

    [HideInInspector] public AnimationCurve rotationCurve;

    [HideInInspector] public float acceleration = 0.03f;

    [HideInInspector] public AnimationCurve accelerationCurve;

    [HideInInspector] public float forwardThreshold = 0.5f;
    [HideInInspector] public float slideThreshold;
    [HideInInspector] public float lowSpeedThreshold;
    [HideInInspector] public float mediumSpeedThreshold;
    [HideInInspector] public float highSpeedThreshold;
    [HideInInspector] public float lowSpeedDamage;
    [HideInInspector] public float mediumSpeedDamage;
    [HideInInspector] public float highSpeedDamage;

    [SerializeField] private WeaponData weaponData;

    public enum Speed
    {
        stop,
        low,
        medium,
        high
    }

    public Speed speedState;



    [Header("Other references")]

    [SerializeField] private TMP_Text hpDisplay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetWeaponStats(weaponData);
    }

    void GetWeaponStats(WeaponData data)
    {
        maxSpeed = data.maxSpeed;
        acceleration = data.maxAcceleration;
        accelerationCurve = data.accelerationCurve;
        maxRotationSpeed = data.maxRotationSpeed;
        rotationCurve = data.rotationCurve;



        lowSpeedDamage = data.lowSpeedDamage;
        mediumSpeedDamage = data.mediumSpeedDamage;
        highSpeedDamage = data.highSpeedDamage;
        lowSpeedThreshold = data.lowSpeedPercent;
        mediumSpeedThreshold = data.mediumSpeedPercent;
        highSpeedThreshold = data.highSpeedPercent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
