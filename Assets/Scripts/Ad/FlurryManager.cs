using UnityEngine;
using System.Collections;

public class FlurryManager : MonoBehaviour {
#if UNITY_ANDROID
	private string FLURRY_API = "9KKW6CMF6BPQ7BDZWF4C";
#elif UNITY_IPHONE
	private string FLURRY_API = "VKQ53X5ZXQC2NHSBNMXD";
#endif
	// Use this for initialization
	static public FlurryManager instance;

	void Start () {
//		FlurryAgent.Instance.onStartSession(FLURRY_API);
	}
	//-----------------
//	FlurryAgent.Instance.logEvent("test update");
	//====================

	void OnDestroy(){
//		FlurryAgent.Instance.onEndSession();
	}
}
