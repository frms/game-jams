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

	public void next() {
		steps [stepIndex].next ();
	}

	public void prev() {
		steps [stepIndex].prev ();
	}

	public void continueBtn() {
		stepIndex++;
		stepIndex %= steps.Length;

		steps [stepIndex].showCurrent ();
	}
}
