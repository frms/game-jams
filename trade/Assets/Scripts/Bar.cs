using UnityEngine;
using System.Collections;

public abstract class Bar : MonoBehaviour {

	public GameObject barPrefab;
	public Vector3 barSize = new Vector3 (1, 1, 1);
	public Vector3 barOffset = new Vector3(-0.5f, 0.65f, 0);
	public Vector3 barRotation = new Vector3 (50, 0, 0);
	public float barProgress = 100;
	public float barMax = 100;
	
	internal Transform bar;
	private Quaternion barOrientation;
	
	// Use this for initialization
	public virtual void Start () {
		barOrientation = Quaternion.Euler (barRotation.x, barRotation.y, barRotation.z);
		GameObject clone = Instantiate(barPrefab, (transform.position + barOffset), barOrientation) as GameObject;
		bar = clone.transform;
		bar.localScale = barSize;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		// Make the bar follow the game obj
		bar.position = transform.position + (barOrientation * barOffset);
		
		// Make the  bar scale to the current barProgress
		bar.localScale = new Vector3((barProgress/barMax) * barSize.x, barSize.y, barSize.z);
	}

}
