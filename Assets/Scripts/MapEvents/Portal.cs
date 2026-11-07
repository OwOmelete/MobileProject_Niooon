using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal ExitPortal;

    private List<Collider2D> PlayersInPortal = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (PlayersInPortal.Contains(other))
            return;

        ExitPortal.TeleportToPortal(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayersInPortal.Remove(other);
    }

    public void TeleportToPortal(Collider2D col)
    {
        if (PlayersInPortal.Contains(col))
            return;

        PlayersInPortal.Add(col);

        col.transform.position = transform.position;
    }
}