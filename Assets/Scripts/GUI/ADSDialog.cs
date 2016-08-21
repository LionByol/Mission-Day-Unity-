using UnityEngine;
using System.Collections;

public class ADSDialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onYes()
	{
		ThirdManager.instance.ShowRewardedVideo();
		PlayerPrefs.SetInt ("sessioncount", 0);
		GameData.sessioncnt = 0;
		gameObject.SetActive (false);
	}

	public void onNo()
	{
		gameObject.SetActive (false);

	}

	public void onAds()
	{
		gameObject.SetActive (false);
	}
}
