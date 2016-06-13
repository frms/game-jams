using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Party : MonoBehaviour
{

    public Transform slotPrefab;
    public Vector2 slotPadding;
    public int numCols;
    public int numRows;

    public Transform[,] grid;
    public Slot[,] slots;

    [System.NonSerialized]
    public Vector2 slotSize;
    [System.NonSerialized]
    public Vector2 firstSlotPos;
    [System.NonSerialized]
    public Vector2 slotDelta;

    public void Awake()
    {
        setUp();

        grid = new Transform[numCols, numRows];
        slots = new Slot[numCols, numRows];

        //Create slots
        Vector2 pos = firstSlotPos;

        for (int i = 0; i < numCols; i++)
        {
            pos.y = firstSlotPos.y;

            for (int j = 0; j < numRows; j++)
            {
                Transform t = Instantiate(slotPrefab, pos, Quaternion.identity) as Transform;
                t.SetParent(transform, false);

                Slot s = t.GetComponent<Slot>();
                s.x = i;
                s.y = j;
                s.parent = this;

                slots[i, j] = s;

                pos.y += slotDelta.y;
            }

            pos.x += slotDelta.x;
        }
    }

	public void setUp ()
    {
        BoxCollider2D col = slotPrefab.GetComponent<BoxCollider2D>();
        slotSize = Vector2.Scale(col.size, slotPrefab.localScale);

        slotDelta = slotSize + slotPadding;

        firstSlotPos = Vector2.zero;
        firstSlotPos.x = slotDelta.x * (numCols - 1) * -0.5f;
        firstSlotPos.y = slotDelta.y * (numRows - 1) * -0.5f;
    }

    public Slot getSlot(Vector2 pos)
    {
        int i = (int)((pos.x - firstSlotPos.x) / slotDelta.x);
        int j = (int)((pos.y - firstSlotPos.y) / slotDelta.y);
        return slots[i, j];
    }

    public void setSlot(Slot s, Transform character)
    {
        setSlot(s.x, s.y, character);
    }

    public void setSlot(int x, int y, Transform character)
    {
        grid[x, y] = character;
        character.SetParent(transform, false);

        Vector3 pos = slotPos(x, y);
        pos += character.localPosition;

        character.localPosition = pos;
    }

    private Vector2 slotPos(int x, int y)
    {
        Vector2 pos = firstSlotPos;
        pos.x += x * slotDelta.x;
        pos.y += y * slotDelta.y;
        return pos;
    }

    public void tryToMove(int x, int y, Transform character)
    {
        if (grid[x, y] == null)
        {
            for (int i = 0; i < numCols; i++)
            {
                for(int j = 0; j < numRows; j++)
                {
                    if (grid[i, j] == character)
                    {
                        grid[i, j] = null;
                        character.localPosition -= (Vector3)slotPos(i, j);
                    }
                }
            }

            setSlot(x, y, character);
        }
    }

    public Transform getRandomChar(bool includeShields = true)
    {
        List<Transform> list = new List<Transform>();

        for(int i = 0; i < numCols; i++)
        {
            for(int j = 0; j < numRows; j++)
            {
                if (grid[i, j] != null && (includeShields || !grid[i, j].name.Contains("Wall")))
                {
                    list.Add(grid[i, j]);
                }
            }
        }

        return random<Transform>(list);
    }

    public Slot getRandomCharSlot(bool includeShields = true)
    {
        return getSlot(getRandomChar(includeShields).localPosition);
    }

    public Slot getRandomEmptySlot(int col = -1, int row = -1)
    {
        List<Slot> list = new List<Slot>();

        int startX = 0;
        int endX = numCols;

        if(col != -1)
        {
            startX = col;
            endX = col + 1;
        }

        int startY = 0;
        int endY = numRows;

        if (row != -1)
        {
            startY = row;
            endY = row + 1;
        }

        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                if (grid[i, j] == null)
                {
                    list.Add(slots[i, j]);
                }
            }
        }

        return random<Slot>(list);
    }

    private static T random<T>(List<T> list)
    {
        if (list.Count == 0)
        {
            return default(T);
        }
        else
        {
            return list[Random.Range(0, list.Count)];
        }
    }

    public Transform getWeakestChar()
    {
        Transform ret = null;
        float lowestHp = Mathf.Infinity;

        for(int i = 0; i < numCols; i++)
        {
            for(int j = 0; j < numRows; j++)
            {
                Transform t = grid[i, j];
                if(t != null)
                {
                    Health h = t.GetComponent<Health>();

                    if(h.currentHealth < lowestHp)
                    {
                        lowestHp = h.currentHealth;
                        ret = t;
                    }
                }
            }
        }

        return ret;
    }

    public float getHp()
    {
        float total = 0;

        for (int i = 0; i < numCols; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                Transform t = grid[i, j];

                if (t != null)
                {
                    total += t.GetComponent<Health>().currentHealth;
                }
            }
        }

        return total;
    }

    public float getDps()
    {
        float total = 0;

        for (int i = 0; i < numCols; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                Transform t = grid[i, j];

                if (t != null)
                {
                    SingleTarget st = t.GetComponent<SingleTarget>();

                    if(st != null && st.dps > 0)
                    {
                        total += st.dps;
                    }
                }
            }
        }

        return total;
    }
}
