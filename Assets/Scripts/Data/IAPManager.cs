using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPManager : BaseIOSFeaturePreview {
	public static IAPManager instance;

	void Awake() {
		instance=this;
        UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction += OnPurchaseFlowFinishedAction;
        UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnConnectFinished;
	}

	public override void Start(){
		Init ();
	}

	public void Init(){
		UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnBillingConnectFinishedAction;
		UM_InAppPurchaseManager.Instance.Init();
	}

	public void BuyConsumable(string id){		
		UM_InAppPurchaseManager.Instance.Purchase(id);
	}

	public void BuyNonConsumable(string id){
		if(UM_InAppPurchaseManager.Instance.IsProductPurchased(id)) {
			MobileNativeMessage msg = new MobileNativeMessage("Already purchased", "You already purchased this item");
//			GameData.SetRemoveAds(true);
			return;
		}
		UM_InAppPurchaseManager.Instance.Purchase(id);
	}
	  
	public void Restore(){
		IOSInAppPurchaseManager.Instance.restorePurchases();
	}

    private void OnConnectFinished(UM_BillingConnectionResult result)
    {

        if (result.isSuccess)
        {
            Debug.Log("Billing init Success");
        }
        else
        {
            Debug.Log("Billing init Failed");
        }
    }

	private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) {
		/*UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction -= OnPurchaseFlowFinishedAction;*/
		if(result.isSuccess) {
			Debug.Log("Product " + result.product.id + " purchase Success");
		} else  {
			Debug.Log("Product " + result.product.id + " purchase Failed");
            return;
		}
        string name = result.product.id;
        
        //spaceship1 ,coin200, coin600,.. coin 2800
        int bikeId=int.Parse(name.Substring(9));
        if (bikeId < 0 || bikeId > 7)
        {
            Debug.LogError("bad BikeId:" + bikeId.ToString());
            return;
        }
        GameData.selectedBike = bikeId;
        GameData.SetBike(bikeId, true);            
        if (MainUI.instance != null)
        {
            MainUI.instance.UpdateBikeInfo(0);
        }
        
	}
	
	private void OnBillingConnectFinishedAction (UM_BillingConnectionResult result) {
		UM_InAppPurchaseManager.OnBillingConnectFinishedAction -= OnBillingConnectFinishedAction;
		if(result.isSuccess) {
			Debug.Log("IAP Connected");
			foreach(UM_InAppProduct product in UM_InAppPurchaseManager.Instance.InAppProducts) {
				Debug.Log("Id: " + product.id);
				Debug.Log("IsConsumable: " + product.IsConsumable);
				
				Debug.Log("Title: " + product.Title);
				Debug.Log("Description: " + product.Description);		
			}
		} else {
			Debug.Log("IAP Failed to connect");
		}
	}
}
