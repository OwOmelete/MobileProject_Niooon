using UnityEngine;

public class RayTests : MonoBehaviour
{
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);

        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.up, Color.red);

        }
         hit = Physics2D.Raycast(transform.position, Vector2.down);

        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.up, Color.red);

        }

    }
}
