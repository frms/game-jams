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

            Slot s = t.GetComponent<Slot>();
            s.index = i;
            s.parent = this;

            pos.x += slotDelta.x;
        }
	}

    public void setSlot(int index, Transform character)
    {
        slots[index] = character;
        character.SetParent(transform, false);

        Vector3 pos = slotPos(index);
        pos += character.localPosition;

        character.localPosition = pos;
    }

    private Vector2 slotPos(int index)
    {
        Vector2 pos = firstSlotPos;
        pos.x += index * slotDelta.x;
        return pos;
    }

    public void tryToMove(int index, Transform character)
    {
        if (slots[index] == null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == character)
                {
                    slots[i] = null;
                    character.localPosition -= (Vector3)slotPos(i);
                    break;
                }
            }

            setSlot(index, character);
        }
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
