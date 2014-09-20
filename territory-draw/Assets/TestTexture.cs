using UnityEngine;
using System.Collections;

public class TestTexture : MonoBehaviour {
	public int x = 0;
	public int y = 0;

	private Texture2D myTexture;
	private Sprite mySprite;
	
	void Start(){
		mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
		myTexture = mySprite.texture;
	}
	
	// Update is called once per frame
	void Update () {
		Color MyPixel = myTexture.GetPixel(x,y);

		Debug.Log ( myTexture.width + " " + myTexture.height );

		if(MyPixel == Color.red) {
			Debug.Log ("Red");
		} else if(MyPixel == Color.green) {
			Debug.Log ("Green");
		} else if(MyPixel == Color.blue) {
			Debug.Log ("Blue");
		}
	}
}
