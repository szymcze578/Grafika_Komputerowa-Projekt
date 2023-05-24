using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D normalCursor;
    public Vector2 normalCursorHotspot;

    public Texture2D aimCursor;
    public Vector2 aimCursorHotspot;

    public void OnButtonCursorEnter()
    {
        Cursor.SetCursor(normalCursor, normalCursorHotspot, CursorMode.Auto);
    }
}
