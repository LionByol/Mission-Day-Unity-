using UnityEngine;
using System.Collections;

public class LeveEndGUI : MonoBehaviour {

	public UILabel bestScore;
	public UILabel currentScore;
	public UILabel ufoDestory;
	public UILabel stonesDestroy;

    public UISprite levelIcon;
    public GameObject fireworks;
    public UISprite gameOver;
    public UISprite levelup;
    public GameObject restartBtn;
    public GameObject continueBtn;

    public GameObject levelCompleted;
    public GameObject levelFailed;

    public GameObject spinDialog;
    public static LeveEndGUI instance;

	public GameObject playBtn;
	public GameObject rateBtn;
	public GameObject shareBtn;
	public GameObject settingBtn;
	public UILabel rankLabel;

	public Sprite[] insignias;
	public UI2DSprite insignia;

	public GameObject delay30;

    void Awake()
    {
        instance = this;
    }
	void Start () {
        Init(SaveStats.instance._levelComplete);
	}

	void Init(bool levelComplete)
	{
		GameData.Init();
		bestScore.text=GameData.bestScore.ToString();
        currentScore.text = SaveStats.instance._score.ToString();
		ShowRank ();

		LevelData levelData=(LevelData)(LoadData.LevelList[GameData.selectedLevel]);

        ufoDestory.text = GameData.killedUfo.ToString();
		stonesDestroy.text=levelData.greyStone.ToString();
        levelIcon.spriteName = "insignia" + (GameData.selectedLevel).ToString();

		//count session and show message.
		GameData.sessioncnt++;
		if (!ThirdManager.isPurchase)
		{
			if (GameData.sessioncnt < 10) {
				PlayerPrefs.SetInt ("sessioncount", GameData.sessioncnt);
			} else {
				PlayerPrefs.SetString ("StartTime", "0");
				MobileNativeRateUs adsPopUp;
				if (Language.CurrentLanguage () == LanguageCode.ZH) {
					adsPopUp = new MobileNativeRateUs ("您的飞船已全部战损", "看广告获取飞船.");
					adsPopUp.yes = "￥20可获不限数量的飞船";
					adsPopUp.no = "不看广告，等12分钟";
					adsPopUp.later = "看广告";
				} else {
					adsPopUp = new MobileNativeRateUs ("No More Sessions", "Watch ads to get more sessions.");
					adsPopUp.yes = "$2.99 for Unlimited Session";
					adsPopUp.no = "No, Wait 12 minutes";
					adsPopUp.later = "Yes, watch ads";
				}
				adsPopUp.SetAppleId ("");
				adsPopUp.SetAndroidAppUrl ("market://details?id=com.peakrainbow.missionday");
				adsPopUp.OnComplete += OnAdsPopUpClose;

				adsPopUp.Start ();
			}
		}

		//count ads session count and show message.
		int later = PlayerPrefs.GetInt("ratelater", 0);
		if ((GameData.adscnt < 29 && later == 0) || later==-1)
		{
			GameData.adscnt++;
			PlayerPrefs.SetInt ("adscount", GameData.adscnt);
		}
		else
		{
			
		}
	}

	private void OnAdsPopUpClose(MNDialogResult result)
	{
		switch (result)
		{
		case MNDialogResult.RATED:
			ThirdManager.instance.BuyProduct ();
			break;
		case MNDialogResult.REMIND:
			ThirdManager.instance.ShowRewardedVideo();
			AudioController.Stop ("Menu");
			break;
		case MNDialogResult.DECLINED:
			delay30.SetActive (true);
			break;
		}
	}

	public void ShowRank()
	{
		int score = SaveStats.instance._score;
		if (score < 1000) {
			rankLabel.text = Language.Get("None");
			fireworks.gameObject.SetActive(false);  
		}else if (score < 2000) {
			rankLabel.text = Language.Get("Level1");
			insignia.sprite2D = insignias [0];
		}else if (score < 3000) {
			rankLabel.text = Language.Get("Level2");
			insignia.sprite2D = insignias [1];
		}else if (score < 4000) {
			rankLabel.text = Language.Get("Level3");
			insignia.sprite2D = insignias [2];
		}else if (score < 5000) {
			rankLabel.text = Language.Get("Level4");
			insignia.sprite2D = insignias [3];
		}else if (score < 6000) {
			rankLabel.text = Language.Get("Level5");
			insignia.sprite2D = insignias [4];
		}else if (score < 7000) {
			rankLabel.text = Language.Get("Level6");
			insignia.sprite2D = insignias [5];
		}else if (score < 8000) {
			rankLabel.text = Language.Get("Level7");
			insignia.sprite2D = insignias [6];
		}else if (score < 9000) {
			rankLabel.text = Language.Get("Level8");
			insignia.sprite2D = insignias [7];
		}else if (score < 10000) {
			rankLabel.text = Language.Get("Level9");
			insignia.sprite2D = insignias [8];
		}else if (score < 11000) {
			rankLabel.text = Language.Get("Level10");
			insignia.sprite2D = insignias [9];
		}else if (score < 12000) {
			rankLabel.text = Language.Get("Level11");
			insignia.sprite2D = insignias [10];
		}else if (score < 13000) {
			rankLabel.text = Language.Get("Level12");
			insignia.sprite2D = insignias [11];
		}else if (score < 14000) {
			rankLabel.text = Language.Get("Level13");
			insignia.sprite2D = insignias [12];
		}else if (score < 15000) {
			rankLabel.text = Language.Get("Level14");
			insignia.sprite2D = insignias [13];
		}else if (score < 16000) {
			rankLabel.text = Language.Get("Level15");
			insignia.sprite2D = insignias [14];
		}else if (score < 17000) {
			rankLabel.text = Language.Get("Level16");
			insignia.sprite2D = insignias [15];
		}else if (score < 18000) {
			rankLabel.text = Language.Get("Level17");
			insignia.sprite2D = insignias [16];
		}else if (score < 19000) {
			rankLabel.text = Language.Get("Level18");
			insignia.sprite2D = insignias [17];
		}else if (score < 20000) {
			rankLabel.text = Language.Get("Level19");
			insignia.sprite2D = insignias [18];
		}else if (score < 21000) {
			rankLabel.text = Language.Get("Level20");
			insignia.sprite2D = insignias [19];
		}else if (score < 22000) {
			rankLabel.text = Language.Get("Level21");
			insignia.sprite2D = insignias [20];
		}else if (score < 23000) {
			rankLabel.text = Language.Get("Level22");
			insignia.sprite2D = insignias [21];
		}else if (score < 24000) {
			rankLabel.text = Language.Get("Level23");
			insignia.sprite2D = insignias [22];
		}else{
			rankLabel.text = Language.Get("Level24");
			insignia.sprite2D = insignias [23];
		}
	}

}
