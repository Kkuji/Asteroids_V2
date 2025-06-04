using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    private Vector2 input = Vector2.zero;

    public float Horizontal => input.x;
    public float Vertical => input.y;
    public Vector2 Direction => input;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            Vector2 radius = background.sizeDelta / 2f;

            Vector2 clamped = new Vector2(
                Mathf.Clamp(localPoint.x, -radius.x, radius.x),
                Mathf.Clamp(localPoint.y, -radius.y, radius.y)
            );

            input = new Vector2(clamped.x / radius.x, clamped.y / radius.y);

            handle.anchoredPosition = clamped;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}