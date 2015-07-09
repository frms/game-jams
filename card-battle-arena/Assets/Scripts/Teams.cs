using UnityEngine;
using System.Collections;

public static class Teams {
	public static Color ONE = new Color (0.18f, 0.8f, 0.443f);
	public static Color TWO = new Color (0.906f, 0.298f, 0.235f);
}

public interface HasTeam {
	Color team { get; set; }
}