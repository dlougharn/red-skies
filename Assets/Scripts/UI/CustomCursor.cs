using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour
{
    public Texture2D CursorTexture;

    // Use this for initialization
    void Start()
    {
        //var cursorOffset = new Vector2(CursorTexture.width / 2, CursorTexture.height / 2);
        //Cursor.SetCursor(CursorTexture, cursorOffset, CursorMode.Auto);
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
