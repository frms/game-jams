using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreatorController : MonoBehaviour {
	private int stepIndex = 0;
	public StepInspector[] allSteps;

	private Step[] steps;

	// Use this for initialization
	void Start () {
		steps = new Step[allSteps.Length];

		for (int i = 0; i < allSteps.Length; i++) {
			Step s = gameObject.AddComponent<Step>();
			s.setUp(allSteps[i]);
			steps[i] = s;
		}

		steps [stepIndex].showCurrent ();
	}

	private float minSwipeDistX = 20f;
	
	private Vector2 startPos;
	
	void Update()
	{	
		if(Input.GetMouseButtonDown(0)) {
			startPos = Input.mousePosition;
		} else if(Input.GetMouseButtonUp(0)) {
			float swipeDistHorizontal = (new Vector3(Input.mousePosition.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
			
			if (swipeDistHorizontal > minSwipeDistX) 
			{
				float swipeValue = Mathf.Sign(Input.mousePosition.x - startPos.x);
				
				if (swipeValue > 0) //right swipe
					steps [stepIndex].prev ();
				else if (swipeValue < 0) //left swipe
					steps [stepIndex].next ();
				
			}
		}
	}
	
	public void continueBtn() {
		stepIndex++;
		stepIndex %= steps.Length;

		steps [stepIndex].showCurrent ();
	}
}
