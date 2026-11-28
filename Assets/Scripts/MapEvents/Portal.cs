using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal ExitPortal;

    private List<Collider2D> PlayersInPortal = new();
    [SerializeField] private CapsuleCollider2D Collider2D;

    public bool isActive;

    [SerializeField] private GameObject LittlePortalObj;
    [SerializeField] private GameObject PortalObj;

    public float ClosedTime;

    public static event Action OnPortalDisappear;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.CompareTag("Player") || other.CompareTag("OnFoot")))
            return;

        if (PlayersInPortal.Contains(other))
            return;

        isActive = false;
        PortalObj.SetActive(false);
        Collider2D.enabled = false;
        OnPortalDisappear?.Invoke();

        ExitPortal.TeleportToPortal(other);
    }

    public void TeleportToPortal(Collider2D col)
    {
        col.transform.position = transform.position;
        isActive = false;
        PortalObj.SetActive(false);
        Collider2D.enabled = false;
    }

    public void LightPortal()
    {
        isActive = true;
        LittlePortalObj.SetActive(true);
        StartCoroutine(OpenPortalDelay());
    }

    IEnumerator OpenPortalDelay()
    {
        yield return new WaitForSeconds(ClosedTime);
        LittlePortalObj.SetActive(false);
        PortalObj.SetActive(true);
        Collider2D.enabled = true;
    }

    public void ResetPortals()
    {
        isActive = false;
        PortalObj.SetActive(false);
        Collider2D.enabled = false;
        LittlePortalObj.SetActive(false);
    }
    
}