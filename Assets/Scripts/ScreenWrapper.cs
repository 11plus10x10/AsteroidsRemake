using System;
using UnityEngine;

/// <summary>
///     Gives the player ability to appear on other side of the screen after reaching screen edge.
/// </summary>
[Serializable]
public struct Borders 
{
    public float topBorder;
    public float bottomBorder;
    public float leftBorder;
    public float rightBorder;
}

public class ScreenWrapper : MonoBehaviour 
{
    public Borders borders;

    private void Update() 
    {
        var currentPosition = transform.position;
        var currentX = transform.position.x;
        var currentY = transform.position.y;
        
        if (currentX > borders.rightBorder) 
        {
            currentPosition.x = borders.leftBorder;
            transform.position = currentPosition;
        }
        if (currentX < borders.leftBorder) 
        {
            currentPosition.x = borders.rightBorder;
            transform.position = currentPosition;
        }

        if (currentY > borders.topBorder) 
        {
            currentPosition.y = borders.bottomBorder;
            transform.position = currentPosition;
        }

        if (!(currentY < borders.bottomBorder)) return;
        currentPosition.y = borders.topBorder;
        transform.position = currentPosition;
    }
}