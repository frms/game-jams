using UnityEngine;
using System.Collections;

public class DebugDraw : MonoBehaviour {

	public GameObject ring;

	// The ring image is a 1024px x 1024px image and one unity unit is 100px, so
	// the following number is the correct scale for the ring image to appear as
	// one unity unit.
	private float ringDefaultSizeToUnityUnit = 0.09765625f;

	/* Creates the ring prefab at the given position and radius and returns the new game object */
	public GameObject createRing(Vector3 position, float radius) {
		GameObject clone = Instantiate (ring, position, Quaternion.identity) as GameObject;
		clone.transform.localScale = new Vector3 (2 * radius * ringDefaultSizeToUnityUnit, 2 * radius * ringDefaultSizeToUnityUnit, 0);
		return clone;
	}
}
