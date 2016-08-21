using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;
using ChartboostSDK;

public class ThirdManager : MonoBehaviour, IStoreListener {
	static public bool isPurchase=false;
    public static bool isTutorial = false;
	static public ThirdManager instance;
	public GameObject loadingUI;
	public GameObject dialogUI;
	public GameObject delay30;
	[HideInInspector]
	public string lastScene;
	public string currentScene;

	void Awake() {
		if (instance!=null){
			Destroy(this.gameObject); 
			return;
		}
		instance=this;
		DontDestroyOnLoad(this);
	}

//////IAP
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	void Start ()
	{
		isPurchase=PlayerPrefs.GetInt("isPurchase",0)==0?false:true;	

		if (m_StoreController == null)
		{
			InitializePurchasing();
		}
	}

	//--------------------------------- IAP
	public void InitializePurchasing() 
	{
		if (IsInitialized())
		{
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.
//		builder.AddProduct("consumable", ProductType.Consumable);
		// Continue adding the non-consumable product.
//		builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
		// And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
		// if the Product ID was configured differently between Apple and Google stores. Also note that
		// one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
		// must only be referenced here. 
		builder.AddProduct("com.peakrainbow.missionday.sessions", ProductType.Consumable);

		UnityPurchasing.Initialize(this, builder);
	}

	private bool IsInitialized()
	{
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyProduct()
	{
		if (IsInitialized())
		{
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID("com.peakrainbow.missionday.sessions");

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		// A consumable product has been purchased by this user.
		if (string.Equals(args.purchasedProduct.definition.id, "com.peakrainbow.missionday.sessions", System.StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			// The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
			isPurchase = true;
			PlayerPrefs.SetInt ("isPurchase", 1);
			PlayerPrefs.SetString ("StartTime", "0");
			PlayerPrefs.SetInt ("sessioncount", 0);
			GameData.sessioncnt = 0;
			ThirdManager.instance.LoadScene ("Main");
			if(!AudioController.IsPlaying("Menu"))
				AudioController.Play ("Menu");
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here....
		else 
		{
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}

		// Return a flag indicating whether this product has completely been received, or if the application needs 
		// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
		// saving purchased products to the cloud, and when that save is delayed. 
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		LoadScene ("menu");
	}

	//-------------------------------------------------------------------

    void OnEnable()
    {
        Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
		Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
    }

    void OnDisable()
    {
		Chartboost.didCompleteRewardedVideo -= didCompleteRewardedVideo;
		Chartboost.didCloseRewardedVideo -= didCloseRewardedVideo;
    }

	public void LoadScene(string name){
		loadingUI.SetActive(true);
		currentScene = name;
		lastScene=Application.loadedLevelName;
		Application.LoadLevel(name); 
	}

	public void ShowLoading(bool flag){
		loadingUI.SetActive(flag);
	}

	public void ShowChartboost(){
		
	}

    public void ShowRewardedVideo()
	{
        Chartboost.showRewardedVideo(CBLocation.Default);
	}

    void didCompleteRewardedVideo(CBLocation location, int reward)
    {
		GameData.SetCash(200);
		PlayerPrefs.SetString ("StartTime", "0");
		PlayerPrefs.SetInt ("sessioncount", 0);
		GameData.sessioncnt = 0;
		ThirdManager.instance.LoadScene ("Main");
		if(!AudioController.IsPlaying("Menu"))
			AudioController.Play ("Menu");
		LoadScene (currentScene);
    }

	public void didCloseRewardedVideo(CBLocation location)
	{
		ThirdManager.instance.LoadScene (currentScene);
		if(!AudioController.IsPlaying("Menu"))
			AudioController.Play ("Menu");
	}
}
