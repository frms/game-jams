using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public Transform playerSprite;
	public float speed = 3;
	public int maxBounce = 10;

	private int bounceCount = 0;

	// Use this for initialization
	void Start () {
	
	}

	void Update () {           

		if(Input.GetButtonDown("Fire1") && rigidbody2D.velocity.sqrMagnitude == 0) {
			float angle = playerSprite.rotation.eulerAngles.z;

			Vector2 direction = angleToVector(angle);
			//Debug.Log(direction);

			rigidbody2D.velocity = direction * speed;
		}
	}

	private Vector2 angleToVector(float angle) {
		Vector2 result = new Vector2(1,0);

		if(angle < 22.5 || angle >= 337.5) {
			// Face right
			result = new Vector2(1,0);
		} else if(angle < 67.5 && angle >= 22.5) {
			// Face up right
			result = new Vector2(1,1);
		} else if(angle < 112.5 && angle >= 67.5) {
			// Face up
			result = new Vector2(0,1);
		} else if(angle < 157.5 && angle >= 112.5) {
			// Face up left
			result = new Vector2(-1,1);
		} else if(angle < 202.5 && angle >= 157.5) {
			// Face left
			result = new Vector2(-1,0);
		} else if(angle < 247.5 && angle >= 202.5) {
			// Face down left
			result = new Vector2(-1,-1);
		} else if(angle < 292.5 && angle >= 247.5) {
			// Face down
			result = new Vector2(0,-1);
		} else if(angle < 337.5 || angle >= 292.5) {
			// Face down right
			result = new Vector2(1,-1);
		}

		result.Normalize();

		return result;
	}

	void OnCollisionEnter2D(Collision2D coll) {

		Debug.Log (coll.gameObject.tag);
		//Debug.Log ("------------------------------------");

		if (coll.gameObject.tag == "Block") {
			stopPlayer();
		} else if (coll.gameObject.tag == "BoundaryBlock") {
			boundaryBlockCollide(coll);
		}
	}

	private void stopPlayer() {
		rigidbody2D.velocity = Vector2.zero;
		
		//Make the position closest to the nearest 0.5 point
		transform.position = new Vector2(Mathf.Floor(transform.position.x) + 0.5f, Mathf.Floor(transform.position.y) + 0.5f);
		//Debug.Log(transform.position);
	}

	private void boundaryBlockCollide(Collision2D coll) {
		stopPlayer();
		
		List<Direction> possibleDirections = new List<Direction>();
		
		var pos = coll.gameObject.transform.position;

		//Debug.Log(transform.position);
		//Debug.Log(pos);
		
		if(pos.x > transform.position.x && Mathf.Abs(pos.x - transform.position.x) > 0.5) {
			//Debug.Log ("boundary to the right");
			possibleDirections.Add(Direction.Up);
			possibleDirections.Add(Direction.UpLeft);
			possibleDirections.Add(Direction.Left);
			possibleDirections.Add(Direction.DownLeft);
			possibleDirections.Add(Direction.Down);
		} else if(pos.x < transform.position.x && Mathf.Abs(pos.x - transform.position.x) > 0.5) {
			//Debug.Log ("boundary to the left");
			possibleDirections.Add(Direction.Right);
			possibleDirections.Add(Direction.UpRight);
			possibleDirections.Add(Direction.Up);
			possibleDirections.Add(Direction.Down);
			possibleDirections.Add(Direction.DownRight);
		} else if(pos.y < transform.position.y && Mathf.Abs(pos.y - transform.position.y) > 0.5) {
			//Debug.Log ("boundary to the down");
			possibleDirections.Add(Direction.Right);
			possibleDirections.Add(Direction.UpRight);
			possibleDirections.Add(Direction.Up);
			possibleDirections.Add(Direction.UpLeft);
			possibleDirections.Add(Direction.Left);
		} else if(pos.y > transform.position.y && Mathf.Abs(pos.y - transform.position.y) > 0.5) {
			//Debug.Log ("boundary to the up");
			possibleDirections.Add(Direction.Right);
			possibleDirections.Add(Direction.Left);
			possibleDirections.Add(Direction.DownLeft);
			possibleDirections.Add(Direction.Down);
			possibleDirections.Add(Direction.DownRight);
		}
//		foreach(Direction d in possibleDirections) {
//			Debug.Log(d);
//		}

		Direction dir = possibleDirections[(int) (possibleDirections.Count*Random.value)];
		//Debug.Log("Picked direction");
		//Debug.Log(dir);

		Vector2 direction = directionToVector(dir);
		//Debug.Log(direction);
		
		rigidbody2D.velocity = direction * speed;
	}


	enum Direction {Right, UpRight, Up, UpLeft, Left, DownLeft, Down, DownRight};

	private Vector2 directionToVector(Direction dir) {
		Vector2 result = new Vector2(1,0);
		
		if(dir == Direction.Right) {
			// Face right
			result = new Vector2(1,0);
		} else if(dir == Direction.UpRight) {
			// Face up right
			result = new Vector2(1,1);
		} else if(dir == Direction.Up) {
			// Face up
			result = new Vector2(0,1);
		} else if(dir == Direction.UpLeft) {
			// Face up left
			result = new Vector2(-1,1);
		} else if(dir == Direction.Left) {
			// Face left
			result = new Vector2(-1,0);
		} else if(dir == Direction.DownLeft) {
			// Face down left
			result = new Vector2(-1,-1);
		} else if(dir == Direction.Down) {
			// Face down
			result = new Vector2(0,-1);
		} else if(dir == Direction.DownRight) {
			// Face down right
			result = new Vector2(1,-1);
		}
		
		result.Normalize();
		
		return result;
	}
}
