using UnityEngine;
using System.Collections;

public class Stars : MonoBehaviour {

	public float _speed  = 1f;

	void Update () {
		transform.position = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime*_speed);	//Move the stars negatively based on player position to simulate paralax scroll
	}
}