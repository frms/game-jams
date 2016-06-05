using UnityEngine;
using System.Collections;

public abstract class PlayerCharacter : Health
{
    public Color selectedColor;
    
    public bool isSelected = false;

    // Update is called once per frame
    public override void Update()
    {
        setColor();

        updateHealthBar();
    }

    private void setColor()
    {
        if (isSelected)
        {
            sr.color = selectedColor;
        }
        else if (isHovered)
        {
            sr.color = hoverColor;
        }
        else
        {
            sr.color = defaultColor;
        }
    }

    public abstract bool handleInput();

    public static RaycastHit2D raycastAtMouse()
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }
}
