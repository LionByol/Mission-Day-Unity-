using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour {
	 public GameObject progressloadslider;
	 public static string levelname;
	 private UISlider progressLoad;
	
	
	 
	 IEnumerator Start() {
		progressLoad = progressloadslider.GetComponent<UISlider>();
		progressLoad.value = 0.0f;
		
        AsyncOperation async = Application.LoadLevelAsync(LoadingScript.levelname);
        while(!async.isDone)
		{
			float loaded = async.progress;
			progressLoad.value = loaded;
			yield return 0;
		}
    }
}
