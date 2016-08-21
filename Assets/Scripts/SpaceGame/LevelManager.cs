using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public GameObject[] _lives;						//List of life gameobjects containing model (Future update note: make dynamic)
	static  public LevelManager instance;			// GameController is a singleton.	 GameController.instance.DoSomeThing();

	void Awake(){
		instance =  this;
	}
	void OnApplicationQuit() {// Ensure that the instance is destroyed when the game is stopped in the editor.
    	instance = null;
	}
	//Checks player status to correspond with GUI lives
	public void CheckPlayer () {	
//		for(int i=0; i < _lives.Length; i++){	
//			if(i>=Player.instance._lives){		
//				_lives[i].SetActive(false);	
//			}else if(i<=Player.instance._lives){
//				_lives[i].SetActive(true);
//			}	
//		}
	}
}
