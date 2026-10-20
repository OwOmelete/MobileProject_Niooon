using System.Collections;
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
    public float hp;

    [HideInInspector] public bool isImmune;

    [SerializeField] private WeaponData weaponData;

    [SerializeField] private float blinkNumberAnim = 3;

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

    public void takeDamage(float n)
    {
        if (isImmune) return;
        hp -= n;
        UpdateHpDisplay();
        StartCoroutine(damageAnim());
        if (hp < 0)
        {
            hp = 0;
            UpdateHpDisplay();
            Destroy(gameObject);
        }
        else
        {
        }
    }

    IEnumerator damageAnim()
    {
        isImmune = true;
        for (int i = 0; i < blinkNumberAnim; i++)
        {
            //sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            // sr.color = baseColor;
            yield return new WaitForSeconds(0.2f);
        }

        isImmune = false;
    }

    private void UpdateHpDisplay()
    {
        hpDisplay.text = hp.ToString();
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

        forwardThreshold = data.forwardThreshold;
        slideThreshold = data.slideThreshold;

        hp = data.hp;

        UpdateHpDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
