using UnityEngine;
using System.Collections;

public class PlayerCharacter : DmgDealer {
    public Color selectedColor;
    
    public bool isSelected = false;

    // Update is called once per frame
    public override void Update()
    {
        setColor();

        updateHealthBar();

        tryToAtk();
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

    public bool handleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "EnemyChar")
            {
                atkTarget = hit.transform;
            }

            return false;
        }

        return true;
    }
}
