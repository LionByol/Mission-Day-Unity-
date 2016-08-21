using UnityEngine;
using System.Collections;

public class LoadingRotate : MonoBehaviour {

	private Transform tr;
	// Use this for initialization
	void Start () {
		tr = transform;
	}
	
	// Update is called once per frame
	void Update () {
		tr.Rotate(0,0,-40*Time.deltaTime);
	}
}
