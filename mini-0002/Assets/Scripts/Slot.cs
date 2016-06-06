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
}
