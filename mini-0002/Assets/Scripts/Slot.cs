using UnityEngine;
using System.Collections;

public class Slot : Hoverable
{
    public int index;
    public Party parent;

    public void tryToMove(Transform character)
    {
        parent.tryToMove(index, character);
    }
}
