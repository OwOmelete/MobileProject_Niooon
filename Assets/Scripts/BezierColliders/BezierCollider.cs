using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(EdgeCollider2D))]
public class BezierCollider2D : MonoBehaviour
{
    public Vector2 firstPoint = Vector2.zero;
    public Vector2 secondPoint = Vector2.right;

    public Vector2 handlerFirstPoint = Vector2.up;
    public Vector2 handlerSecondPoint = new Vector2(1, 1);

    public int pointsQuantity = 5;

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 handlerP0, Vector3 handlerP1, Vector3 p1)
    {
        float u = 1.0f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term
        p += 3f * uu * t * handlerP0; //second term
        p += 3f * u * tt * handlerP1; //third term
        p += ttt * p1; //fourth term

        return p;
    }

    public Vector2[] calculate2DPoints()
    {
        List<Vector2> points = new List<Vector2>();

        points.Add(firstPoint);
        for (int i = 1; i < pointsQuantity; i++)
        {
            points.Add(CalculateBezierPoint((1f / pointsQuantity) * i, firstPoint, handlerFirstPoint, handlerSecondPoint, secondPoint));
        }
        points.Add(secondPoint);

        return points.ToArray();
    }

}