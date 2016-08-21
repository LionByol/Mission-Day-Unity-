using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(GoGarage());
	}
	IEnumerator GoGarage(){
		yield return new WaitForSeconds(1.5f);
		ThirdManager.instance.LoadScene("Garage");
	}
}
