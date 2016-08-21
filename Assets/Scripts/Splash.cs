using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameData.vState = 3;
		if (PlayerPrefs.GetInt("Init",0)==0){
			PlayerPrefs.SetInt("Init",1);
			Application.LoadLevel("GamePlay");
		}
		else
			Application.LoadLevel("Home");
		GameData.missileRate = 0.04f;
		GameData.startScore = 0;
	}
}
