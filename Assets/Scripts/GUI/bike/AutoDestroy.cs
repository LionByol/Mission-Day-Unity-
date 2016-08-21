using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public int time=2;
	// Use this for initialization
	void Start () {	
		StartCoroutine("Destroy");
	}

	IEnumerator Destroy()
	{
		yield return new WaitForSeconds(time);
		GameObject.Destroy(this.gameObject);
	}
}
