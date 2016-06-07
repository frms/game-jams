using UnityEngine;
using System.Collections;

public class Slot : Hoverable
{
    public int x;
    public int y;
    public Party parent;

    public void tryToMove(Transform character)
    {
        parent.tryToMove(x, y, character);
    }

    public bool isOpen()
    {
        return parent.grid[x, y] == null;
    }

    public void set(Transform character)
    {
        parent.setSlot(x, y, character);
    }
}
