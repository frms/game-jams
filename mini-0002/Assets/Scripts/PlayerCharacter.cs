using UnityEngine;
using System.Collections;

public class PlayerCharacter : Health
{
    public Color selectedColor;
    
    public bool isSelected = false;

    private SingleTarget singleTarget;

    public override void Start()
    {
        base.Start();

        singleTarget = GetComponent<SingleTarget>();
    }

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

    public bool handleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "EnemyChar")
            {
                singleTarget.target = hit.transform;
            }

            return false;
        }

        return true;
    }
}
