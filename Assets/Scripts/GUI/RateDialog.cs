using UnityEngine;
using System.Collections;

public class RateDialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onRate()
	{
		PlayerPrefs.SetInt ("ratelater", 0);		
		GameData.adscnt = 0;
		gameObject.SetActive (false);
	}

	public void onLater()
	{
		PlayerPrefs.SetInt ("ratelater", 1);
		gameObject.SetActive (false);
	}

	public void onNotRemind()
	{
		PlayerPrefs.SetInt ("ratelater", -1);
		gameObject.SetActive (false);
	}
}
