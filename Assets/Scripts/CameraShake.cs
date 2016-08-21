using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	private Vector3 camPOS;
	private Quaternion camAng;
	public bool is2D;
	public float quake=0.2f; 
	public static bool startShake = false;

	void Start() {
		camPOS = transform.position;	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(startShake){
//			transform.position = Random.insideUnitSphere * quake;	
//			if(is2D) 
//				transform.position = new Vector3(transform.position.x,transform.position.y,camPOS.z);
//			else
				transform.localPosition = camPOS+ Random.insideUnitSphere * quake;
		}
	}
	
	public void ShakeFor(float a){
		StartCoroutine(WaitForSecond(a));
	}

	public void FinishShake(){
		if (startShake){
			startShake = false;
			transform.localPosition = camPOS;
			transform.localRotation=camAng;
		}
	}

 	IEnumerator WaitForSecond(float a) {
		camPOS = transform.localPosition;
		camAng=transform.localRotation;
		startShake = true;
 		yield return new WaitForSeconds(a);
		FinishShake();
	}
}
