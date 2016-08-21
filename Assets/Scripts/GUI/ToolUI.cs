using UnityEngine;
using System.Collections;

public class ToolUI : MonoBehaviour {

	public GameObject storeWnd;
	public GameObject mainWnd;

	public HWnd curHwnd=HWnd.Main;
	[HideInInspector]
	public HWnd beforeH=HWnd.Main;

	public Texture2D textureForPost;

	public GameObject shareBtn;
	public TweenPosition sharePos;

	public static ToolUI instance;

	void Awake(){
		instance=this;
	}
	string url;
	void Start () {
		ThirdManager.instance.ShowLoading(false);
		url="Hey, check out this new app, Fast Fours, it's pretty fun. \n\n ";
		switch(Application.platform) {
		case RuntimePlatform.Android:
			url+="https://play.google.com/store/apps/details?id=com.peakrainbow.missionday";
			break;
		case RuntimePlatform.IPhonePlayer:
			url+="https://itunes.apple.com/app/id90657372?mt=8";
			break;
		}
	}

	public void OnStoreBtn(){
		curHwnd=HWnd.Store;
		ShowMainUI(false);
		storeWnd.SetActive(true);  
	}
	
	public void OnBackBtn(){
		if (curHwnd==HWnd.Stage){

			ShowMainUI(true);
			curHwnd=HWnd.Main;
		}
		else if (curHwnd==HWnd.Main){
			ThirdManager.instance.LoadScene("Home");
		}
		else if (curHwnd==HWnd.Store){
			storeWnd.SetActive(false);
			ShowMainUI(true);
			curHwnd=HWnd.Main;
		}
	}

	void ShowMainUI(bool flag){
		mainWnd.SetActive(flag);
	}

	// When a user click 10000 coin purchaes button
	public void OnPurchase1(string id){  
		IAPManager.instance.BuyNonConsumable(id);
	}

	public void OnContact(){
		UM_ShareUtility.SendMail( "Fast Fours Support", "Hello Fast Fours Support. <br><br> Please tell us what type of device you have? and how we can help you.", "Support@softwarestudios.com");
	}

	
	public void OnShare(){
		shareBtn.SetActive(false);
		sharePos.PlayForward();
	}

	public void OnFBShare(){
		OnShareClose();
		UM_ShareUtility.FacebookShare(url, textureForPost);
	}

	public void OnTwShare(){
		OnShareClose();
		UM_ShareUtility.TwitterShare(url, textureForPost);
	}

	public void OnShareClose(){
		sharePos.PlayReverse();
		shareBtn.SetActive(true);
	}
}
