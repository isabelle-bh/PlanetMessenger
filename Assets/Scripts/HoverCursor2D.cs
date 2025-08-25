using UnityEngine;
using UnityEngine.EventSystems;

public class HoverCursor2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D hoverCursor;
    public Texture2D normalCursor;
    public Vector2 hotspot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(hoverCursor, hotspot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
    }
}