using UnityEngine;
using System.Collections;

public enum HWnd
{
	Main,
	Store,
	Task,
	Lottery,
	Stage,
    Level,
	Help
}

public class MainUI : MonoBehaviour {
    public static bool isSelectLevel = false;
	public GameObject StageSelectWnd;
	public GameObject storeWnd;
	public GameObject taskWnd;
	public GameObject mainWnd;
	public GameObject helpWnd;
	public GameObject WorldCamera;
	public HWnd curHwnd=HWnd.Main;
	[HideInInspector]
	public HWnd beforeH=HWnd.Main;

	public UILabel coin;
	public UILabel score;
	public UISprite[] star;
	public UISprite level;
	   
	public UILabel price;
    public UILabel IAPprice;

	public UILabel speed;
	public UILabel health;
	public UILabel force;
	public UISprite speedBar;
	public UISprite healthBar;
	public UISprite controlSpeedBar;

	public GameObject buyBtn;
	public GameObject startBtn;
    public GameObject tryBtn;
    public GameObject rBuyBtn;

	public static MainUI instance;

	void Awake(){
		instance=this;
	}

	void Start () {
		if (GameData.isPlaying)
			OnStartBtn();
		StartCoroutine(InitGarage());
		ThirdManager.instance.ShowLoading(false);
		score.text=GameData.bestScore.ToString();

        if (isSelectLevel)
        {
            OnStartBtn();
            isSelectLevel = false;
        }
            
	}

	IEnumerator InitGarage(){
		yield return new WaitForEndOfFrame();
		coin.text=GameData.coin.ToString();
		UpdateBikeInfo(0);
	}

	public void OnStartBtn(){
		ThirdManager.instance.LoadScene("Main");
	}

	public void OnRaceBtn(){
		AudioController.Stop("Menu");
		ThirdManager.instance.LoadScene("Battle");
	}

	public void OnStoreBtn(){
		curHwnd=HWnd.Store;
		StageSelectWnd.SetActive(false);
		taskWnd.SetActive(false);

		ShowMainUI(false);
		storeWnd.SetActive(true);
	}

	public void OnTaskBtn(){
		curHwnd=HWnd.Task;
		ShowMainUI(false);
		taskWnd.SetActive(true);
	}

	public void OnLotteryBtn(){
		curHwnd=HWnd.Lottery;
		ShowMainUI(false);
	}

	public void OnHelp(){
		curHwnd=HWnd.Help;
		ShowMainUI(false);
		helpWnd.SetActive(true);
	}

	public void OnRightArrowBtn(){
		UpdateBikeInfo(1);	
	}

	public void OnLeftArrowBtn(){
		UpdateBikeInfo(-1);
	}

	public void UpdateBikeInfo(int d){
		star[GameData.selectedBike].spriteName="noStar";
		BikerGarageRotate.instance.bikeList[GameData.selectedBike].SetActive(false);
		GameData.selectedBike=(GameData.selectedBike+d+7)%7;
		PlayerPrefs.SetInt("selectedBike",GameData.selectedBike);
		star[GameData.selectedBike].spriteName="star";
		BikerGarageRotate.instance.bikeList[GameData.selectedBike].SetActive(true) ;
		BikeData bike=(BikeData)(LoadData.BikeList[GameData.selectedBike]);
		level.spriteName=bike.level;

        if (GameData.selectedBike > 0)
        {
            price.text = bike.price.ToString();
            UM_InAppProduct product = UM_InAppPurchaseManager.Instance.GetProductById("spaceship" + GameData.selectedBike.ToString());
            IAPprice.text = "$ " + product._price;
        }

		speed.text=bike.speed.ToString()+"m/s";
		health.text=bike.health.ToString();
		force.text=bike.force.ToString();
		speedBar.fillAmount=bike.speed/40f;
		healthBar.fillAmount=(bike.health)/500;
		controlSpeedBar.fillAmount=bike.force/20f;

		buyBtn.SetActive(!GameData.GetBike(GameData.selectedBike));
      	rBuyBtn.SetActive(!GameData.GetBike(GameData.selectedBike));
		startBtn.SetActive(GameData.GetBike(GameData.selectedBike));	
	}

	public void OnPurchaseBtn(){
		int val=int.Parse(price.text);
		if (GameData.coin>=val){
			GameData.SetCoin(-val);
			coin.text=GameData.coin.ToString();
			GameData.SetBike(GameData.selectedBike,true);
			buyBtn.SetActive(false);
            rBuyBtn.SetActive(false);
			startBtn.SetActive(true);
		}
	}

    public void OnPurchaseIAP()
    {
        IAPManager.instance.BuyNonConsumable("spaceship" + GameData.selectedBike.ToString());        
    }

	public void OnBackBtn(){
		if (curHwnd==HWnd.Stage){
			StageSelectWnd.SetActive(false);
			ShowMainUI(true);
			curHwnd=HWnd.Main;
		}
		else if (curHwnd==HWnd.Main){
			ThirdManager.instance.LoadScene("Main");
		}
		else if (curHwnd==HWnd.Store){
			storeWnd.SetActive(false);
			ShowMainUI(true);
			curHwnd=HWnd.Main;
		}
		else if (curHwnd==HWnd.Task){
			taskWnd.SetActive(false);
			ShowMainUI(true);
			curHwnd=HWnd.Main;
		}
		else if (curHwnd==HWnd.Lottery){

			ShowMainUI(true);		
			curHwnd=HWnd.Main;
		}
		else if (curHwnd==HWnd.Help){
			helpWnd.SetActive(false);		
			ShowMainUI(true);		
			curHwnd=HWnd.Main;
		}
	}

	void ShowMainUI(bool flag){
		WorldCamera.SetActive(flag);
		mainWnd.SetActive(flag);
	}

	// When a user click 10000 coin purchaes button
	public void OnPurchase1(){  
		purchase (10000);
	}
	// When a user click 100000 coin purchaes button
	public void OnPurchase2(){  
		purchase (100000);
	}
	// When a user click 1000000 coin purchaes button
	public void OnPurchase3(){  
		purchase (1000000);
	}
	// When a user click 5000000 coin purchaes button
	public void OnPurchase4(){  
		purchase (5000000);
	}

	// handle purchase coin part 
	void purchase(int val){
		GameData.SetCoin(val);  //add coin
		coin.text=GameData.coin.ToString();  // change coin value in UI
	}
}
