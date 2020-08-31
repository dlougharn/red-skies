using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public Texture2D CursorTexture;

    private Image _cursorImage;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        _cursorImage = GetComponent<Image>();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
