using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public CustomCursor paintCursor;

    private void Start()
    {
        //paintCursor.SetSpriteVisible(false);
    }

    private void OnEnable()
    {
        paintCursor.SetSpriteVisible(true);
    }
    private void OnDisable()
    {
        paintCursor.SetSpriteVisible(false);
    }


    public void OnMouseOver()
    {
        Cursor.visible = false;
        //paintCursor.MoveCursor();
    }

    public void OnMouseExit()
    {
        Cursor.visible = true;
        //paintCursor.SetSpriteVisible(false);
    }
    public void OnMouseEnter()
    {
        Cursor.visible = false;
        paintCursor.SetSpriteVisible(true);
    }


}

