using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	const string PREFS_HIGHSCORE = "HighScore";

	const string PREFS_EFFECTS = "Effects";
	const string PREFS_MUSIC = "Music";

	public int menuState=0;
	
	[HideInInspector]
	public int prefHighScore = 0;
	[HideInInspector]
	public int prefCoins = 0;
	[HideInInspector]
	public float prefOptionEffects = 1.0f;
	[HideInInspector]
	public float prefOptionMusic = 1.0f;

	public static GameManager instance;
    public static bool levelResult;
	
	bool ready = false;
	public bool isReady { get { return ready; } }
	
	public int curLevel=0;
	static public string mapName;

	static public bool isJoyStick=true;
	static public int TankLevel;
	
	
	static public int exp;
	static public float maxExp;
	public int[] countEnemy;
	static public int diedEnemyCount=0;
	
	void Start () {
		if (instance != null) {
			Destroy(gameObject);
			return ;
		}
		instance = this;
		countEnemy= new int[4];
		LoadPrefs();
		ready = true;
	}
	
	void LoadPrefs() {
		prefHighScore = PlayerPrefs.GetInt(PREFS_HIGHSCORE);
		prefCoins=PlayerPrefs.GetInt("coin", 0);

		TankLevel=PlayerPrefs.GetInt("tankLevel", 1);

		exp=PlayerPrefs.GetInt("exp",0);
		maxExp=TankLevel*200+100;

		prefOptionEffects = PlayerPrefs.GetFloat(PREFS_EFFECTS, 1.0f);
		prefOptionMusic = PlayerPrefs.GetFloat(PREFS_MUSIC, 1.0f);
	}

	public void SaveScore(int score){   
		prefHighScore+=score;
		exp+=score;
		PlayerPrefs.SetInt(PREFS_HIGHSCORE, prefHighScore);
		PlayerPrefs.SetInt("exp", exp);

		int lev=(int)(prefHighScore/(200+TankLevel*100))+1;
		if (exp>maxExp){		
			TankLevel++;
			exp-=(int)maxExp;
			maxExp=TankLevel*200+1;
			if (exp>maxExp)
				exp=TankLevel*50;
			PlayerPrefs.SetInt("exp", exp);
			PlayerPrefs.SetInt("tankLevel", TankLevel);
		}

	}
	
	public void SaveCoins(int coin) { //old param, int coins
		prefCoins+=coin;
		PlayerPrefs.SetInt("coin", prefCoins);
	}

	public void SaveAudio(float volume) {
		prefOptionEffects = volume;
		PlayerPrefs.SetFloat(PREFS_EFFECTS, prefOptionEffects);
		PlayerPrefs.Save();
//		SoundManager.ApplyVolume();
	}
	
	public void SaveMusic(float volume) {
		prefOptionMusic = volume;
		PlayerPrefs.SetFloat(PREFS_MUSIC, prefOptionMusic);
		PlayerPrefs.Save();
//		SoundManager.ApplyVolume();
	}

	public void EndLevel(){
		menuState=1;
		Application.LoadLevel("Garage");
	}
}
