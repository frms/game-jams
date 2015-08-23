using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public GameObject barPrefab;
	public Vector3 barSize = new Vector3 (1, 1, 1);
	public Vector3 barOffset = new Vector3(0f, 0f, 0);
	public Vector3 barRotation = new Vector3 (0, 0, 0);
	public float barProgress = 100;
	public float barMax = 100;

	public bool isPlayer = false;
	
	private Transform bar;
	private Quaternion barOrientation;
	
	// Use this for initialization
	void Start () {
		if(isPlayer) {
			bar = barPrefab.transform;
		} else {
			barOrientation = Quaternion.Euler (barRotation.x, barRotation.y, barRotation.z);
			GameObject clone = Instantiate(barPrefab, (transform.position + barOffset), barOrientation) as GameObject;
			bar = clone.transform;
			bar.localScale = barSize;
		}

//		SpriteRenderer sr = bar.GetComponent<SpriteRenderer> ();
//		sr.color = GetComponent<TeamMember>().teamId;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isPlayer) {
			// Make the bar follow the game obj
			bar.position = transform.position + (barOrientation * barOffset);
		}
		
		// Make the  bar scale to the current barProgress
		bar.localScale = new Vector3((barProgress/barMax) * barSize.x, barSize.y, barSize.z);
	}

	public void applyDamage(float damage) {
		barProgress -= damage;
		
		// Kill the game obj if it loses all its health
		if(barProgress <= 0) {
			Destroy(gameObject);
			Destroy(bar.gameObject);
		}
	}

	void OnDestroy() {
		if (bar != null) {
			Destroy (bar.gameObject);
		}
	}
}
