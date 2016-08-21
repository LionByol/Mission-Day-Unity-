using UnityEngine;
using System.Collections;

public class SelectMap : MonoBehaviour {
	public GameObject loadingObject;	
	public UILabel level;

	void OnEnable () {
		level.text="Level "+GameManager.TankLevel.ToString();
	}
	
	public void OnSelectMap(){
//		FlurryAgent.Instance.logEvent(GameManager.mapName);
		loadingObject.SetActive(true);
		Application.LoadLevel(GameManager.instance.curLevel+2);
	}

	public void BackMapButton() {
		ThirdManager.instance.LoadScene("MainMenu");
	}
}
