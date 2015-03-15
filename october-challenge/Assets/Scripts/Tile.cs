using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public Color color;

	public float changeChance;

	// Use this for initialization
	void Awake () {
		/* Set the scale to the tile size */
		transform.localScale = new Vector3 (Grid.tileSize, Grid.tileSize, 1);
	}

	public void setUp(Color color, float changeChance) {
		this.color = color;
		renderer.material.color = color;
		this.changeChance = changeChance;
	}

	public void setUp(Texture2D tex) {
		renderer.material.mainTexture = tex;
	}
}
