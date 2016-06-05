using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Party : MonoBehaviour
{

    public Transform slotPrefab;
    public Vector2 slotPadding;
    public int numSlots;

    public Transform[] slots;
    
    public Vector2 slotSize
    {
        get
        {
            BoxCollider2D col = slotPrefab.GetComponent<BoxCollider2D>();
            return Vector2.Scale(col.size, slotPrefab.localScale);
        }
    }

    private Vector2 firstSlotPos;
    private Vector2 slotDelta;

    void Awake()
    {
        slots = new Transform[numSlots];

        slotDelta = slotSize + slotPadding;

        firstSlotPos = Vector2.zero;
        firstSlotPos.x = slotDelta.x * (numSlots - 1) * -0.5f;
    }

	// Use this for initialization
	void Start ()
    {
        Vector2 pos = firstSlotPos;

	    for(int i = 0; i < numSlots; i++)
        {
            Transform t = Instantiate(slotPrefab, pos, Quaternion.identity) as Transform;
            t.SetParent(transform, false);

            pos.x += slotDelta.x;
        }
	}

    public void setSlot(int i, Transform character)
    {
        slots[i] = character;
        character.SetParent(transform);

        Vector3 pos = slotPos(i);
        pos += character.position;

        character.localPosition = pos;
    }

    private Vector2 slotPos(int i)
    {
        Vector2 pos = firstSlotPos;
        pos.x += i * slotDelta.x;
        return pos;
    }

    public Transform getRandomChar()
    {
        List<Transform> list = new List<Transform>();

        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i] != null)
            {
                list.Add(slots[i]);
            }
        }

        if(list.Count == 0)
        {
            return null;
        }
        else
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}
