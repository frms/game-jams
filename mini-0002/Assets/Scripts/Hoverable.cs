using UnityEngine;
using System.Collections;

public class Hoverable : MonoBehaviour
{
    public Color defaultColor;
    public Color hoverColor;

    internal bool isHovered = false;

    internal SpriteRenderer sr;

    // Use this for initialization
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isHovered)
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
}
