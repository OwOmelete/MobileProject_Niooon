using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public PlayerController controller;
    
    [Header("Health/Damage")]
    [SerializeField] private float damageMult = 3;
    [SerializeField] private float blinkNumberAnim = 3;

    public float hp = 20;

    private bool isImmune = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isImmune) return;
        if (other.CompareTag("Pointe"))
        {
            HealthSystem point = other.GetComponent<HealthSystem>();
            if (controller.CurrentSpeed < point.controller.CurrentSpeed)
            {
                takeDamage(CalculateDamage(controller.CurrentDirection * controller.CurrentSpeed,
                    point.controller.CurrentDirection * point.controller.CurrentSpeed));
            }
            
        }
        else if(other.CompareTag("Player"))
        {
            HealthSystem opponentPoint = other.GetComponentInChildren<HealthSystem>();
            opponentPoint.takeDamage(CalculateDamage(opponentPoint.controller.CurrentDirection * opponentPoint.controller.CurrentSpeed,
                controller.CurrentDirection * controller.CurrentSpeed));
        }
        
    }

    private float CalculateDamage(Vector2 playerVector, Vector2 opponentVector)
    {
        float f = Mathf.Abs (playerVector.magnitude - opponentVector.magnitude);
        return (f * damageMult)+5;
    }

    private bool isTriggeredFromBehind(Collider2D other)
    {
        Vector2 direction = other.transform.position - transform.position;
        return Vector2.Dot(transform.up, direction) <= 0;
    }
    
    public void takeDamage(float n)
    {
        if (isImmune) return;
        hp -= n;
        StartCoroutine(damageAnim());
        if (hp < 0)
        {
            hp = 0;
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
}
