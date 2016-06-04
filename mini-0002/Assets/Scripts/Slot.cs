using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour
{
    public Color defaultColor;
    public Color selectedColor;
    public Color hoverColor;

    public bool isSelected = false;
    private bool isHovered = false;

    private SpriteRenderer sr;


    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
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

    void OnMouseEnter()
    {
        isHovered = true;
    }

    void OnMouseExit()
    {
        isHovered = false;
    }

    void OnMouseDown()
    {
        isSelected = true;
    }
}
