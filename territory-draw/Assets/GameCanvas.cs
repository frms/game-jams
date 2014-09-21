using UnityEngine;
using System.Collections;

public class GameCanvas : MonoBehaviour {
	public int x = 0;
	public int y = 0;
	
	private Texture2D tex;
	
	void Start(){
		tex = (Texture2D) renderer.material.mainTexture;

		clearTexture();
	}
	
	// Update is called once per frame
	void Update () {
//		Color myPixel = tex.GetPixel(x,y);
//		
//		Debug.Log ( myTexture.width + " " + myTexture.height );
//		
//		if(MyPixel == Color.red) {
//			Debug.Log ("Red");
//		} else if(MyPixel == Color.green) {
//			Debug.Log ("Green");
//		} else if(MyPixel == Color.blue) {
//			Debug.Log ("Blue");
//		} else if(MyPixel == Color.white) {
//			Debug.Log("White");
//		}

		clickPixel();
	}

	private void clickPixel ()
	{
		if (!Input.GetMouseButton (0))
			return;

		RaycastHit hit;
		if (!Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
			return;
		
		Renderer renderer = hit.collider.renderer;
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
			return;

		Vector2 pixelUV = hit.textureCoord;
		print ((int)(pixelUV.x * tex.width) + "--" + (int)(pixelUV.y * tex.height));

		tex.SetPixel((int) (pixelUV.x * tex.width), (int) (pixelUV.y * tex.height), Color.red);

		tex.Apply();
	}

	private void clearTexture() {
		Color[] pixels = new Color[tex.width*tex.height];

		for(int i = 0; i < pixels.Length; i++) {
			pixels[i] = Color.white;
		}

		tex.SetPixels(pixels);

		tex.Apply();
	}
}
