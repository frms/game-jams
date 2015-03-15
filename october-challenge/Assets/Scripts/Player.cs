using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 5;
	public Vector2 targetPosition;

	private Vector2 startPosition;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Awake () {
		// Clone the current texture so we don't modify the original one. I don't know why unity acts like this.
		Texture2D tex = Instantiate (renderer.material.mainTexture) as Texture2D;
		renderer.material.mainTexture = tex;

		// Don't destroy the player when we switch to the game over screen
		DontDestroyOnLoad(this.gameObject);
	}

	void Start () {
		moveTo(transform.position);
	}

	public void moveTo(Vector2 pos) {
		startPosition = transform.position;
		targetPosition = pos;

		startTime = Time.time;
		journeyLength = Vector2.Distance(startPosition, targetPosition);
	}

	void Update() {
		if(journeyLength > 0) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;

			Vector2 newPos = Vector2.Lerp(startPosition, targetPosition, fracJourney);
			transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
		}
	}

	public bool isMoving() {
		return transform.position.x != targetPosition.x || transform.position.y != targetPosition.y;
	}
}
