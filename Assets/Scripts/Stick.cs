using System;
using UnityEngine;
using UnityEngine.UI;

public class Stick : MonoBehaviour
{
    public Vector2 Direction;
    public Vector2 Position;

    public RectTransform handleTransform;
    public Image handleImage;

    private int currentTouch;

    public float StickRange;

    public float MaxRangeX;
    public float MaxRangeY;

    public Collider2D stickZone;
    public RectTransform stickZoneTransform;
    
    
    public RectTransform parent;

    public Vector2 startPosition;

    private void Start()
    {
        startPosition = parent.anchoredPosition;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                LayerMask mask = LayerMask.GetMask("UI");
                RaycastHit2D hit = Physics2D.Raycast(touch.position, Vector2.zero, mask);
                if (currentTouch == touch.fingerId)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Moved:
                            HandleTouchMoved(touch.position);
                            break;
                        case TouchPhase.Ended:
                            HandleTouchEnded(touch.position);
                            break;
                    }

                    return;
                    
                }
                else
                {
                    if (isInRange(touch.position.x, touch.position.y))
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            currentTouch = touch.fingerId;
                            HandleTouchBegan(touch.position);
                        }
                    }
                }
            }
        }
    }

    private void HandleTouchEnded(Vector2 touchPosition)
    {
        currentTouch = -1;
        Direction = Vector2.zero;
        handleImage.enabled = false;
        MoveImage();
        
    }

    private void HandleTouchMoved(Vector2 touchPosition)
    {
        Position = new Vector2(touchPosition.x - parent.transform.position.x,
            touchPosition.y - parent.transform.position.y);
        Direction = (Position + parent.anchoredPosition) - parent.anchoredPosition;
        StickSlide();
        
        MoveImage();
    }

    private void HandleTouchBegan(Vector2 touchPosition)
    {
        handleImage.enabled = true;
        Position = new Vector2(touchPosition.x - parent.transform.position.x,
            touchPosition.y - parent.transform.position.y);
        Direction = (Position + parent.anchoredPosition) - parent.anchoredPosition;        
        
        MoveImage();
    }

    private void MoveImage()
    {
        handleTransform.anchoredPosition = Position;
    }

    private void StickSlide()
    {
        if (Direction.magnitude > StickRange)
        {
            parent.anchoredPosition += Direction - Direction.normalized * StickRange;
            parent.anchoredPosition = new Vector2( Mathf.Clamp(parent.anchoredPosition.x,startPosition.x - MaxRangeX, startPosition.x + MaxRangeX),
                Mathf.Clamp(parent.anchoredPosition.y,startPosition.y - MaxRangeY, startPosition.y + MaxRangeY));
        }
    }

    private bool isInRange(float x, float y)
    {
        return (x < stickZone.transform.position.x + MaxRangeX &&
            x > stickZone.transform.position.x - MaxRangeX && 
            y < stickZone.transform.position.y + MaxRangeY &&
            y > stickZone.transform.position.y - MaxRangeY) ;
    }
    
}
