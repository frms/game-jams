using UnityEngine;
using System.Collections;

public class PlayerCharacter : Health {
    public Color selectedColor;
    
    public bool isSelected = false;

    public Bullet bulletPrefab;
    public float atkRate;

    private Transform atkTarget = null;
    private float lastAtkTime = -Mathf.Infinity;

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

        updateHealthBar();

        if(atkTarget != null && lastAtkTime + atkRate < Time.time)
        {
            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as Bullet;
            b.target = atkTarget;

            lastAtkTime = Time.time;
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
