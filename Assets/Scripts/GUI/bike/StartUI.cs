using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour {
	public GameObject dlgObj;
	public GameObject settingModule;
	public UILabel titleLbl;
	public UILabel textLbl;
	public GameObject delay30;

	string url;

	void Start(){
        ThirdManager.isTutorial = false;
		if (!AudioController.IsPlaying("Menu"))
			AudioController.Play("Menu");
		AudioController.Instance.Volume = PlayerPrefs.GetFloat("Music", 1.0f);
		ThirdManager.instance.ShowLoading(false);
		ThirdManager.isPurchase=PlayerPrefs.GetInt("isPurchase",0)==0?false:true;

        url = "You have completed your mission. Share it with friends? \n\n ";

 		switch(Application.platform) {
 		case RuntimePlatform.Android:
			url+="https://play.google.com/store/apps/details?id=com.peakrainbow.missionday";
 			break;
 		case RuntimePlatform.IPhonePlayer:
 			url+="https://itunes.apple.com/app/id?mt=8";
 			break;
 		}

		if (!ThirdManager.isPurchase) {
			int delay = PlayerPrefs.GetInt ("sessioncount", 0);
			if (delay >= 9) {
				if (PlayerPrefs.GetString ("StartTime", "0") != "0")
					delay30.SetActive (true);
				else {
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
			ThirdManager.instance.ShowRewardedVideo ();
			AudioController.Stop ("Menu");
			break;
		case MNDialogResult.DECLINED:
			delay30.SetActive (true);
			break;
		}
	}

	public void ClickStartButton(){
		#if (!UNITY_EDITOR)
		if (!ThirdManager.isPurchase) {
			if (GameData.sessioncnt >= 9) {
				ThirdManager.instance.LoadScene("Main");
				return;
			}
		}
		#endif
		GameData.selectedBike=4;
		GameData.selectedLevel = 10;
		AudioController.Stop("Menu");
		ThirdManager.instance.LoadScene("Battle");
	}

	public void OnSettingButton(){
		titleLbl.text=Language.Get("GameSettings");
		settingModule.SetActive(true);
		dlgObj.SetActive(true);	
	}

	public void OnHelpButton(){
        ThirdManager.isTutorial = true;
        GameData.selectedLevel=10;
        GameData.selectedBike=4;
        ThirdManager.instance.LoadScene("Battle");
	}
    
	public void OnInfoButton(){
		titleLbl.text=Language.Get("About");
		textLbl.gameObject.SetActive(true);
		dlgObj.SetActive(true);
	}

	public void OnCloseBtn(){
		dlgObj.SetActive(false); 
		textLbl.gameObject.SetActive(false);
		settingModule.SetActive(false);
	}

	public void OnContact(){
		UM_ShareUtility.SendMail("Hello", "market://details?id=com.peakrainbow.missionday", "newcomer33@gmail.com");
	}
	
	
	public void OnFBShare(){	
		UM_ShareUtility.FacebookShare("market://details?id=com.peakrainbow.missionday");//, textureForPost);
	}
	
	public void OnTwShare(){
		UM_ShareUtility.TwitterShare("market://details?id=com.peakrainbow.missionday");//, textureForPost);
	}

	public void onRate()
	{
		Application.OpenURL ("market://details?id=com.peakrainbow.missionday");
	}

	public void onShare()
	{
		if(!Application.isEditor)
		{
			string shareText  = "Shooting Game Share";
			string gameLink = "Download the game on play store at \n"+url;
			string subject = "Shooting Game";
			string details = "Let's do shooting game widely.";

			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), details +"\n"+ gameLink);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

			currentActivity.Call("startActivity", intentObject);
		}
	}

	public void onCustomizeButotn()
	{
		SceneManager.LoadScene ("customize");
	}
}
