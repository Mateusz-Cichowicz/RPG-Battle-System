using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Texture2D mainCursor;

    void Start()
    {
        SetMainCursor();
    }
    public void SetMainCursor() 
    {
        if (mainCursor != null)
        {
            Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        else
        {
            Debug.LogWarning("Main cursor not assigned in the Unity Editor.");
        }
    }
    public void ChooseCursor(Texture2D cursor) 
    {
        Debug.Log("Inside cursor controler");
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
