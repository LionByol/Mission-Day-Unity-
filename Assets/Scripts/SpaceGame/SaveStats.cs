using UnityEngine;
using System.Collections;

public class SaveStats : MonoBehaviour {
	//Script saves score to game over scene
	public int _score ;
	public string _previousLevel;
	public bool _levelComplete = true;
	public float _saveSoundVol ;
	public float _saveMusicVol ;

	static public SaveStats instance  ;			//SaveStats is a singleton.	 SaveStats.instance.DoSomeThing();

	void Awake(){
		instance=this;
	}

	void OnApplicationQuit() {				//Ensure that the instance is destroyed when the game is stopped in the editor.
	    instance = null;
	}

	void Start () {
		if (instance){
	        Destroy (gameObject);			//Destroy if there is a SaveStats loaded
	    }else{
	        instance = this;				
	        DontDestroyOnLoad (gameObject); //Keep from deleting this gameObject when loading a new scene
	    }
	}
}