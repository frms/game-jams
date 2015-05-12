using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GroupBtn : MonoBehaviour, IDeselectHandler {
	private Image img;
	private GameManager gm;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void click() {
		if (img.color == Color.yellow) {
			EventSystem.current.SetSelectedGameObject (null);
		} else {
			img.color = Color.yellow;
		}
	}
	
	public void OnDeselect (BaseEventData data) 
	{
		img.color = Color.white;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		if (Physics.Raycast (ray, out hit)){
			//print (hit.transform.tag);
			if(hit.transform.tag == "EnemyCity") {
				gm.groupOneTarget = hit.transform;
			} else if(hit.transform.tag == "PlayerCity") {
				gm.groupOneTarget = null;
			}
		}
	}
}
