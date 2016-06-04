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

    void OnMouseDown()
    {
        isSelected = true;
    }
}
