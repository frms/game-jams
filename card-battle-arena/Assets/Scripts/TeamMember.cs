using UnityEngine;
using System.Collections;

public class TeamMember : MonoBehaviour  {
	public static Color TEAM_1 = new Color (0.18f, 0.8f, 0.443f);
	public static Color TEAM_2 = new Color (0.906f, 0.298f, 0.235f);

	public Color teamId;

	[System.NonSerialized]
	public GameObject highlight;

	public Vector3 highlightSize = new Vector3 (1.1f, 1.1f, 1f);

	public virtual void Start () {
		highlight = new GameObject ("Highlight");

		Transform t = highlight.transform;
		t.localScale = highlightSize;
		t.SetParent (transform, false);

		SpriteRenderer sr = highlight.AddComponent<SpriteRenderer> ();
		sr.sprite = GetComponent<SpriteRenderer> ().sprite;
		sr.color = new Color (0.161f, 0.502f, 0.725f);

		highlight.SetActive (false);
	}
}