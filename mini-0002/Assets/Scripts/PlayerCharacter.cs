using UnityEngine;
using System.Collections;

public class PlayerCharacter : Hoverable {
    public Color selectedColor;
    
    public bool isSelected = false;

    // Update is called once per frame
    public override void Update()
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
                Debug.Log("ATK!! " + hit.collider.name);
            }

            return false;
        }

        return true;
    }
}
