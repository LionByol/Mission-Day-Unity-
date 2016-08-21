using UnityEngine;
using System.Collections;

public class CameraRate : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_IPHONE
//		this.gameObject.transform.localScale=new Vector3(0.8f,0.8f,1f);
//		print (iPhone.generation);
		if (UnityEngine.iOS.Device.generation==UnityEngine.iOS.DeviceGeneration.iPad1Gen || 
		    UnityEngine.iOS.Device.generation==UnityEngine.iOS.DeviceGeneration.iPad2Gen || 
		    UnityEngine.iOS.Device.generation==UnityEngine.iOS.DeviceGeneration.iPad3Gen || 
		    UnityEngine.iOS.Device.generation==UnityEngine.iOS.DeviceGeneration.iPad4Gen || 
		    UnityEngine.iOS.Device.generation==UnityEngine.iOS.DeviceGeneration.iPad5Gen || 
		    UnityEngine.iOS.Device.generation==UnityEngine.iOS.DeviceGeneration.iPadMini1Gen || 
		    UnityEngine.iOS.Device.generation==UnityEngine.iOS.DeviceGeneration.iPadMini2Gen)
		{
			this.gameObject.transform.localScale=new Vector3(0.8f,0.8f,1f);
		}
#endif
//		Camera    camera = this.GetComponent<Camera>(); 
//		print (Screen.width+":"+ Screen.height);
//		float perx = 641.0f / Screen.width;
//		float pery = 427.0f / Screen.height;
//		float v = (perx > pery) ? perx : pery;
//		camera.orthographicSize = v; 
	}

}
