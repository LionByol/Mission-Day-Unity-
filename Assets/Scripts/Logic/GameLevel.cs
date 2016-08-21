using UnityEngine;
using System.Collections;

public class GameLevel : MonoBehaviour {
	[HideInInspector]
	static public int currentTime;
	void Awake(){
		int level=GameData.selectedLevel;
		Instantiate(Resources.Load("Prefab/_Game Controller Level"));
		Instantiate(Resources.Load("Prefab/Player"+GameData.selectedBike.ToString()));
	}

	IEnumerator Start () {
		while (GameManager.instance == null || GUIManager.instance==null) {
			yield return null;
		}			
		
		Init();
	}
	
	void Init(){
		currentTime=580;
		GameManager.diedEnemyCount=0;
		for (int i=0;i<4;i++)
			GameManager.instance.countEnemy[i]=0;
	}

	IEnumerator Timer()
	{
		while(true)
		{	
			yield return new WaitForSeconds(1f);
			currentTime-=1;
			if (currentTime<0){
				StartCoroutine(GUIManager.instance.ShowGameOverLabel(1));				
			}
        }
	}
}
