using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour {
	public static Color TEAM_1 = new Color (0.18f, 0.8f, 0.443f);
	public static Color TEAM_2 = new Color (0.906f, 0.298f, 0.235f);

	public Color team;

	public bool[,] mapCollider = new bool[,] {
		{ true, true, true },
		{ true, true, true },
		{ true, true, true }
	};
}
