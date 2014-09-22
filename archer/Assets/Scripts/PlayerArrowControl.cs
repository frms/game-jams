using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Collections.ObjectModel;

public class PlayerArrowControl : MonoBehaviour {

	public GameObject projectile;
	public Transform spawnPoint;


	public Transform bowDrawBar;
	public float bowDrawMaxTime = 1;
	private Vector3 bowDrawScale;
	private float bowDrawStartTime = -1;

	public float arrowSpeed = 10;

	public float maxDistance = Mathf.Infinity;

	public bool playerControlsDistance = false;

	public float numberOfPossibleTeleportArrows = Mathf.Infinity;
	private LinkedList<Arrow> shotArrows = new LinkedList<Arrow>();
	private int nextArrowId = 1;
	private LinkedListNode<Arrow> teleportArrowNode;
	private StringBuilder teleportArrowsText = new StringBuilder();
	
	public bool randomTeleport = false;

	public bool arrowsPassThroughAll = false;

	public bool cameraFollowsArrow = false;
	private CameraMovement cameraMovement;

	public bool teleporteeFollowsArrow = false;

	public bool teleportOnArrowLand = false;
	private bool lastFrameInFlight = false;

	public float delayBeforeTeleport = 0f;


	// Use this for initialization
	void Start () {
		// Save the initial full local scale of the bow drawing bar
		bowDrawScale = bowDrawBar.localScale;

		// Hide the bow drawing bar by default
		bowDrawBar.localScale = Vector3.zero;

		// Initialize the camera movement object
		cameraMovement = Camera.main.GetComponent<CameraMovement> ();
	}

	private void fireArrow() {
		fireArrow (maxDistance);
	}

	private void fireArrow(float distance) {
		GameObject clone = Instantiate (projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;

		ArrowProperties props = new ArrowProperties ();
		props.id = nextArrowId;
		props.maxDistance = distance;
		props.isTrigger = arrowsPassThroughAll;
		clone.SendMessage ("setUp", props);

		nextArrowId++;

		// Set the velocity of the projectile in the local down direction of the player,
		// because thats the direction he faces
		clone.rigidbody2D.velocity = (-1 * transform.up) * arrowSpeed;

		changeTeleportArrowNode( shotArrows.AddLast(clone.GetComponent<Arrow>()) );

		// Make sure we dont have too many arrows that we can teleport to
		while (shotArrows.Count > numberOfPossibleTeleportArrows) {
			shotArrows.RemoveFirst();
		}
	}

	/* Determines how far the bow has been drawn */
	private float bowDrawPercent() {
		return Mathf.Min( (Time.time - bowDrawStartTime), bowDrawMaxTime ) / bowDrawMaxTime;
	}

	private void randomizeSelectedTeleportArrow() {
		int index = Random.Range (0, shotArrows.Count);

		LinkedListNode<Arrow> node = shotArrows.First;

		for (int i = 1; i <= index; i++) {
			node = node.Next;
		}

		changeTeleportArrowNode(node);
	}

	/* Helper function to set the teleportArrowNode to a new arrow */
	private void changeTeleportArrowNode(LinkedListNode<Arrow> node) {
		teleportArrowNode = node;

		// Since we are changing the arrow make sure we reset lastFrameInFlight to false.
		lastFrameInFlight = false;
	}

	/* Helper function to teleport to an arrow */
	private void teleportToArrow(Arrow arrow) {
		StartCoroutine(waitToTeleportToArrow(arrow, delayBeforeTeleport));
	}

	private IEnumerator waitToTeleportToArrow(Arrow arrow, float waitTime) {
		yield return new WaitForSeconds(waitTime);

		teleportToArrowImmediately(arrow);
	}

	private void teleportToArrowImmediately(Arrow arrow) {
		transform.position = arrow.transform.position;
	}

	void Update() {

		updateTeleportArrowListAndText ();

		// If the player can control how far the arrows can go and we have a non infinite max distance for the arrows
		if (playerControlsDistance && maxDistance != Mathf.Infinity) {
			// Save the start time of the bow being drawn
			if(Input.GetButtonDown("Fire1")) {
				bowDrawStartTime = Time.time;
			}

			// Animate the bow draw bar
			if(Input.GetButton("Fire1") && bowDrawStartTime != -1) {
				bowDrawBar.localScale = new Vector3(bowDrawPercent() * bowDrawScale.x, bowDrawScale.y, bowDrawScale.z);
			} else {
				bowDrawBar.localScale = Vector3.zero;
			}

			// Fire the arrow on release
			if(Input.GetButtonUp("Fire1") && bowDrawStartTime != -1) {
				fireArrow(bowDrawPercent() * maxDistance);

				bowDrawStartTime = -1;
			}
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				fireArrow();
			}
		}
		
		if (Input.GetButtonDown ("Fire2") && teleportArrowNode != null) {
			if(randomTeleport) {
				randomizeSelectedTeleportArrow();
			}

			// Teleport to the current selected arrow's position
			teleportToArrow(teleportArrowNode.Value);
		}

		// Handle the camera following 
		if (cameraFollowsArrow && shotArrows.Last != null) {
			Arrow lastArrow = shotArrows.Last.Value;

			if(lastArrow.inFlight) {
				cameraMovement.target = lastArrow.transform;
			} else {
				cameraMovement.target = transform;
			}
		} else {
			cameraMovement.target = transform;
		}

		// Handle the teleportee following the arrow
		if (teleporteeFollowsArrow && shotArrows.Last != null) {
			Arrow lastArrow = shotArrows.Last.Value;
			
			if(lastArrow.inFlight) {
				teleportToArrow(lastArrow);
			}
		}
		
		// Handle changing the selected teleport arrow
		if (teleportArrowNode != null) {
			// If Q is pressed go to the previous arrow in the list
			if(Input.GetKeyDown(KeyCode.Q)) {
				changeTeleportArrowNode( (teleportArrowNode.Previous != null) ? teleportArrowNode.Previous : shotArrows.Last );
			}
			// If E is pressed go to the next arrow in the list
			else if(Input.GetKeyDown(KeyCode.E)) {
				changeTeleportArrowNode( (teleportArrowNode.Next != null) ? teleportArrowNode.Next : shotArrows.First );
			}
		}

		// If we are supposed to teleport on arrow landing and we have a currently selected teleport arrow
		if (teleportOnArrowLand && teleportArrowNode != null) {
			Arrow arrow = teleportArrowNode.Value;

			// Teleport when the arrow goes from "in flight" last frame to "not in flight" this frame
			if(arrow.inFlight == false && lastFrameInFlight == true) {
				teleportToArrow( arrow );
			}

			// Update the last frame in flight for the arrow
			lastFrameInFlight = arrow.inFlight;
		}

	}

	private void updateTeleportArrowListAndText() {
		teleportArrowsText = new StringBuilder();

		// Loop through the list of shot arrows, if the arrow's transform is not null then add
		// it to the text. If it is null then remove it from the list.
		LinkedListNode<Arrow> node = shotArrows.First;

		while (node != null) {
			LinkedListNode<Arrow> next = node.Next;

			if(node.Value != null) {
				// If the current node is the selected teleport arrow then make its text yellow
				if(node == teleportArrowNode) {
					teleportArrowsText.Append("<color=yellow>"+node.Value.id+"</color>");
				}
				// Else just add it to the list as normal
				else {
					teleportArrowsText.Append(node.Value.id);
				}
				teleportArrowsText.Append(" ");
			} else {
				// If we are about to remove the selected teleport arrow then make the previous arrow the new selected arrow
				if(node == teleportArrowNode) {
					changeTeleportArrowNode(teleportArrowNode.Previous);
				}
				shotArrows.Remove(node);
			}

			node = next;
		}
	}


	float native_width = 1920;
	float native_height = 1080;
	
	void OnGUI ()
	{
		Matrix4x4 saveMat = GUI.matrix; // save current matrix

		//set up scaling
		float rx = Screen.width / native_width;
		float ry = Screen.height / native_height;
		GUI.matrix = Matrix4x4.TRS (new Vector3(0, 0, 0), Quaternion.identity, new Vector3 (rx, ry, 1)); 

		// Do normal gui code from here on as though the resolution is guarenteed to be the native resolution

		GUIStyle defaultStyle = new GUIStyle ();
		defaultStyle.richText = true;
		defaultStyle.fontSize = 52;

		string text = teleportArrowsText.ToString ();
		Vector2 size = defaultStyle.CalcSize(new GUIContent(text));
		
		GUI.Label(new Rect(10, 10, size.x, size.y), text, defaultStyle);

		// Finish doing gui code

		GUI.matrix = saveMat; // restore matrix
	}
}
