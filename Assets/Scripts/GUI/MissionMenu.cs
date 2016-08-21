using UnityEngine;
using System.Collections;

public class MissionMenu : MonoBehaviour {	
	public static MissionMenu instance;
    public string appleId = "";
	public string apdroidAppUrl = "market://details?id=com.peakrainbow.missionday";

	void Start () {
        ThirdManager.instance.ShowLoading(false);
		instance=this;

		AudioController.Stop ("Music");
		if (!AudioController.IsPlaying("Menu"))
			AudioController.Play("Menu");

        GameData.playCnt++;
        PlayerPrefs.SetInt("playCnt", GameData.playCnt);
        if (!SaveStats.instance._levelComplete) // if success
        {
            MobileNativeDialog dialog = new MobileNativeDialog("Share!", "You have completed this mission. Share it with friends?");
            dialog.OnComplete += OnDialogClose;
        }

		if (GameData.playCnt % 30 == 0 && GameData.playCnt>0)
        {
			MobileNativeRateUs ratePopUp;
			if (Language.CurrentLanguage () == LanguageCode.ZH) {
				ratePopUp = new MobileNativeRateUs("诚求您的反馈", "您的反馈对我们意味着很多.");
				ratePopUp.yes = "现在行动";
				ratePopUp.later = "以后再说";
				ratePopUp.no = "惨忍拒绝";
			} else {
				ratePopUp = new MobileNativeRateUs("Rate Mission Day", "Please take a moment to help us by providing your feedback. Thank you for your support.");
			}
            ratePopUp.SetAppleId(appleId);
            ratePopUp.SetAndroidAppUrl(apdroidAppUrl);
            ratePopUp.OnComplete += OnRatePopUpClose;

            ratePopUp.Start();
        }
	}

	public void BackButton(){
		ThirdManager.instance.LoadScene("Main");
	}

    public void NextLevelButton()
    {
        GameData.selectedLevel++;
        ThirdManager.instance.LoadScene("Battle");
    }

    public void RestartButton()
    {
        ThirdManager.instance.LoadScene("Battle");
    }

    public void SpaceshipButton()
    {
        ThirdManager.instance.LoadScene("Garage");
    }

	public void LoadGarage(){
		ThirdManager.instance.LoadScene("Garage");
	}

    private void OnDialogClose(MNDialogResult result)
    {
        switch (result)
        {
            case MNDialogResult.YES:
                Debug.Log("Yes button pressed");
                break;
            case MNDialogResult.NO:
                Debug.Log("No button pressed");
                break;
        }
    }

    private void OnRatePopUpClose(MNDialogResult result)
    {
		GameData.playCnt = PlayerPrefs.GetInt("playCnt", 0);
        switch (result)
        {
			case MNDialogResult.RATED:	
				Application.OpenURL ("market://details?id=com.peakrainbow.missionday");	
				GameData.playCnt = 1;
                break;
			case MNDialogResult.REMIND:		
				GameData.playCnt = 1;
                break;
			case MNDialogResult.DECLINED:
				Debug.Log ("Declined Option pickied");
				GameData.playCnt = -10000;
				PlayerPrefs.SetInt("playCnt", GameData.playCnt);;
                break;
        }
    }

	public void onPlay()
	{
		#if (!UNITY_EDITOR)
		if (!ThirdManager.isPurchase) {
			if (GameData.sessioncnt >= 10) {
				ThirdManager.instance.LoadScene("Menu");
				return;
			}
		}
		#endif
		ThirdManager.instance.LoadScene("Battle");
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
			string gameLink = "Download the game on play store at \n http://play.google.com/store/apps/details?id=com.peakrainbow.missionday";
			string subject = "Shooting Game";
			string details = "Current Score: " + SaveStats.instance._score + "\n" + "Best Score: " + GameData.bestScore;

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

	public void onSetting()
	{
		ThirdManager.instance.LoadScene("Main");
	}
}
