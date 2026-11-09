using System;
using UnityEngine;
using UnityEngine.UI;

public class Stick : MonoBehaviour
{
    [HideInInspector] public Vector2 Direction;
    [HideInInspector]public Vector2 Position;

    public RectTransform handleTransform;
    public Image handleImage;

    private int currentTouch = -1;

    [SerializeField] private float StickRange;

    [SerializeField] private float InnerMaxRangeX;
    [SerializeField] private float InnerMaxRangeY;
    [SerializeField] private float OuterMaxRangeX;
    [SerializeField] private float OuterMaxRangeY;
    [SerializeField] private float InnerMaxRangeRadius;
    [SerializeField] private float OuterMaxRangeRadius;
    [SerializeField] private bool IsCircle;

    public Collider2D stickZone;
    public RectTransform stickZoneTransform;
    
    public RectTransform parent;

    public Vector2 startPosition;

    private float canvaScale;
    
    private void Start()
    {
        startPosition = parent.anchoredPosition;
        
        canvaScale = parent.GetComponentInParent<Canvas>().scaleFactor;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                //LayerMask mask = LayerMask.GetMask("UI");
                //RaycastHit2D hit = Physics2D.Raycast(touch.position, Vector2.zero, mask);
                if (currentTouch == touch.fingerId)
                {
                    if (IsCircle)
                    {
                        if (isInRangeCircle(touch.position.x, touch.position.y))
                        {
                            switch (touch.phase)
                            {
                                case TouchPhase.Moved:
                                    HandleTouchMoved(touch.position);
                                    break;
                                case TouchPhase.Ended:
                                    HandleTouchEnded();
                                    break;
                            }
                        }
                        else
                        {
                        
                            HandleTouchEnded();
                        }
                        return;
                    }
                    else
                    {
                        if (isInRange(touch.position.x, touch.position.y))
                        {
                            switch (touch.phase)
                            {
                                case TouchPhase.Moved:
                                    HandleTouchMoved(touch.position);
                                    break;
                                case TouchPhase.Ended:
                                    HandleTouchEnded();
                                    break;
                            }
                        }
                        else
                        {
                        
                            HandleTouchEnded();
                        }
                        return;
                    }
                    
                }
                else
                {
                    if (IsCircle)
                    {
                        if (isInRangeCircle(touch.position.x, touch.position.y))
                        {
                            if (touch.phase == TouchPhase.Began)
                            {
                                currentTouch = touch.fingerId;
                                HandleTouchBegan(touch.position);
                            }
                        }
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
    }

    private void HandleTouchEnded()
    {
        currentTouch = -1;
        Direction = Vector2.zero;
        handleImage.enabled = false;
        MoveImage();
        
    }

    private void HandleTouchMoved(Vector2 touchPosition)
    {
        Position = (touchPosition - (Vector2)parent.position) / canvaScale;
        Direction = (Position + parent.anchoredPosition) - parent.anchoredPosition;
        if (IsCircle)
        {
            StickSlideCircle();
        }
        else
        {
            StickSlide();
        }
        
        
        MoveImage();
    }

    private void HandleTouchBegan(Vector2 touchPosition)
    {
        handleImage.enabled = true;

        Position = Vector2.zero;
        Direction = Vector2.zero;

        RectTransform parentRect = parent.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                parentRect,
                touchPosition,
                null,
                out Vector3 worldPosition))
        {
            parent.position = worldPosition;
        }
        handleTransform.anchoredPosition = Vector2.zero;
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
            parent.anchoredPosition = new Vector2( Mathf.Clamp(parent.anchoredPosition.x,startPosition.x - InnerMaxRangeX, startPosition.x + InnerMaxRangeX),
                Mathf.Clamp(parent.anchoredPosition.y,startPosition.y - InnerMaxRangeY, startPosition.y + InnerMaxRangeY));
        }
    }

    private void StickSlideCircle()
    {
        if (Direction.magnitude > StickRange)
        {
            parent.anchoredPosition += Direction - Direction.normalized * StickRange;

            Vector2 offset = parent.anchoredPosition - startPosition;

            if (offset.magnitude > InnerMaxRangeRadius)
            {
                offset = offset.normalized * InnerMaxRangeRadius;
            }

            parent.anchoredPosition = startPosition + offset;
        }
    }
    
    private bool isInRange(float x, float y)
    {
        return (x < stickZone.transform.position.x + OuterMaxRangeX &&
            x > stickZone.transform.position.x - OuterMaxRangeX && 
            y < stickZone.transform.position.y + OuterMaxRangeY &&
            y > stickZone.transform.position.y - OuterMaxRangeY) ;
    }

    private bool isInRangeCircle(float x, float y)
    {
        Vector2 center = stickZone.transform.position;
        Vector2 position = new Vector2(x, y);

        return (position - center).sqrMagnitude < OuterMaxRangeRadius * OuterMaxRangeRadius;
    }
}
