using UnityEngine;
using System.Collections;

public class Party : MonoBehaviour {

    public Transform slotPrefab;
    public Vector2 slotPadding;
    public int numSlots;
    
    public Vector2 slotSize
    {
        get
        {
            BoxCollider2D col = slotPrefab.GetComponent<BoxCollider2D>();
            return Vector2.Scale(col.size, slotPrefab.localScale);
        }
    }

	// Use this for initialization
	void Start () {
        Vector2 delta = slotSize + slotPadding;

        Vector2 pos = Vector2.zero;
        pos.x = delta.x * ( numSlots - 1 ) * -0.5f;

	    for(int i = 0; i < numSlots; i++)
        {
            Transform t = Instantiate(slotPrefab, pos, Quaternion.identity) as Transform;
            t.SetParent(transform, false);

            pos.x += delta.x;
        }
	}
}
