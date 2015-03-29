using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	public Transform beam;

	private Transform user;

	void Start() {
		user = transform.parent;
	}

	public void use() {
		Instantiate (beam, transform.position, user.localRotation);
	}
}
