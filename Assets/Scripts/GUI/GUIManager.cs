using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIManager : MonoBehaviour {
	public static GUIManager instance;	
	public GameObject gameOverLabel;
	//-------Time and Enemy Tank Count Died
    public int coin=0;
    public UILabel coinLabel;
	public UILabel scoreLabel;
	public UILabel stoneLabel;
	public UILabel enemyLabel;   
	public UILabel liveLabel;
	public UILabel sessionLabel;
	public UILabel missileLabel;

	public UILabel ufonumber;

//---------------
	void Start()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}
		instance = this;
		if (!AudioController.IsPlaying("Music"))
			AudioController.Play("Music");

		sessionLabel.text = (GameData.sessioncnt+1)+"";
	}

	public IEnumerator ShowGameOverLabel(int type)
	{
		GameObject obj=GUIManager.instance.gameOverLabel;
		obj.SetActive(true);
		if (type==1)
			obj.GetComponent<UILabel>().text="time_out";				
		obj.GetComponent<TweenScale>().enabled=true;
		yield return new WaitForSeconds(3f);
		GameManager.instance.EndLevel();
	}

	void Update()
	{
		missileLabel.text = GameData.specMissiles + "";
	}
	
	public void ShowScore(int value)
	{
		scoreLabel.text = value.ToString ();
	}

	public void ShowLive(int value)
	{
		liveLabel.text = value.ToString ();
	}

	public void ShowStone(int value, int totalValue){
		string val = "";
		val += value + "/" + totalValue;
		stoneLabel.text = val;
	}

	public void ShowEnemy(int value, int totalValue){
		string val = "";
		val += value + "/" + totalValue;
		enemyLabel.text = val;
	}

	public void OnPause(){
		Time.timeScale = 0;
	}

	public void RunAgainClicked(){
		Time.timeScale = 1f;
	}
	
	public void ButtonQuit(){
		Time.timeScale = 1f;
		ThirdManager.instance.LoadScene ("Main");
	}

    public void GetCash(int dd)
    {
        if (!ThirdManager.isTutorial)
        {
            coin += dd;
        }        
    }
}
