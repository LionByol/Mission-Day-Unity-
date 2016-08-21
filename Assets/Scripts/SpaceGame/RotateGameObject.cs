using UnityEngine;
using System.Collections;

public class RotateGameObject : MonoBehaviour {
	//Simple script to rotate a gameObject
	public float xRot ;
	public float yRot ;
	public float zRot;

	void Update () {
		transform.Rotate(new Vector3(xRot,yRot,zRot)*Time.deltaTime);
	}
}