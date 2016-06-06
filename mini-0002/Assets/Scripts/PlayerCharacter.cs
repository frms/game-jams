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

    public virtual bool handleInput()
    {
        return handleMove();
    }

    public bool handleMove()
    {
        bool stillControlling = true;

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = PlayerCharacter.raycastAtMouse();

            if (hit.collider != null && hit.collider.tag == "Slot")
            {
                Slot s = hit.collider.GetComponent<Slot>();

                if (s.parent == BattleManager.main.playerParty)
                {
                    s.tryToMove(transform);
                }
            }

            stillControlling = false;
        }

        return stillControlling;
    }

    public static RaycastHit2D raycastAtMouse()
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }
}
