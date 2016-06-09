using UnityEngine;
using System.Collections;

public class PauseOverlay : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        Vector3 scale = 2 * Camera.main.ViewportToWorldPoint(Vector3.one);
        scale.z = 1;
        transform.localScale = scale;
	}
}
