using UnityEngine;
using System.Collections;

public class DialogUI : MonoBehaviour {

	void Start () {
	
	}

	public void OnOk(){
		Application.Quit();
	}

	public void OnCancel(){
		ThirdManager.instance.dialogUI.SetActive(false);
	}
}
