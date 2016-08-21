using UnityEngine;
using System.Collections;

public class ShowAd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (ThirdManager.isPurchase)
			return;
		string curScene=Application.loadedLevelName;
		if (curScene=="MainMenu") // show banner
		{

		}		
	}
}
