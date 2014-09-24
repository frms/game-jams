using UnityEngine;
using System.Collections;

public class GameCanvas : MonoBehaviour {
	public int x = 0;
	public int y = 0;

	private Vector2 originWorldCoords;
	private Texture2D tex;
	
	void Start(){
		// Clone the current texture so we don't modify the original one. I don't know why unity acts like this.
		tex = Instantiate(renderer.material.mainTexture) as Texture2D;
		renderer.material.mainTexture = tex;

		// Calculate the world coordinates of the texture's 0,0 position
		float x = transform.position.x - (transform.localScale.x / 2);
		float y = transform.position.y - (transform.localScale.y / 2);
		originWorldCoords = new Vector3 (x, y);
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

	public void drawColor(Vector2 worldPosition, Color color) {
		//Debug.Log (worldPosition);

		Vector2 offsetCoords = worldPosition - originWorldCoords;
		int x = (int) ( (offsetCoords.x / transform.localScale.x) * tex.width );
		int y = (int) ( (offsetCoords.y / transform.localScale.y) * tex.height );

		//Debug.Log (texCoords);

		// Draw a square at the current location
		Color[] colors = new Color[4];
		for(int i = 0; i < 4; i++) {
			colors[i] = color;
		}
		tex.SetPixels (x-1, y-1, 2, 2, colors);

		tex.Apply ();
	}

}
