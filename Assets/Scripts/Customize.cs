using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Customize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GameData.vState == 1) {
			v1.value = true;
			v2.value = false;
			v3.value = false;
		} else if (GameData.vState == 2) {
			v1.value = false;
			v2.value = true;
			v3.value = false;
		} else {
			v1.value = false;
			v2.value = false;
			v3.value = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnOk()
	{
		GameData.startScore = int.Parse( startscore.value );
		GameData.missileRate = missiles.value;
		SceneManager.LoadScene ("Main");
	}

	public void OnChangeMissiles()
	{
		valuelab.text = missiles.value*0.1f+"";
	}

	public void OnV1()
	{
		if (v1.value)
		{
			GameData.vState = 1;
		}
	}
	public void OnV2()
	{
		if (v2.value)
		{
			GameData.vState = 2;
		}
	}
	public void OnV3()
	{
		if (v3.value)
		{
			GameData.vState = 3;
		}
	}

	public UIToggle v1;
	public UIToggle v2;
	public UIToggle v3;
	public UISlider missiles;
	public UIInput startscore;
	public UILabel valuelab;
}
