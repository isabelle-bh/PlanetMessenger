using UnityEngine;
using UnityEngine.EventSystems;

public class HoverCursor3D : MonoBehaviour
{
    public Texture2D hoverCursor;
    public Texture2D normalCursor;
    public Vector2 hotspot = Vector2.zero;

    void OnMouseEnter()
    {
        Cursor.SetCursor(hoverCursor, hotspot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
    }
}