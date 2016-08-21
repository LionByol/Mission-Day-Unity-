using UnityEngine;
using System.Collections;

public class OptionsGUI : MonoBehaviour {
	
	public UISlider sliderAudio;
	public UISlider sliderMusic;	
	string[] qualityNames;
	
	void OnEnable () {
		float iii=GameManager.instance.prefOptionEffects;
		sliderAudio.sliderValue = iii;
		sliderMusic.sliderValue = GameManager.instance.prefOptionMusic;
	}
	
	public void OnAudioChange() {
//		print ("change"+sliderAudio.sliderValue.ToString());
		GameManager.instance.SaveAudio(sliderAudio.sliderValue);
	}
	
	public void OnMusicChange() {
		GameManager.instance.SaveMusic(sliderMusic.sliderValue);
		AudioController.Instance.Volume = sliderMusic.sliderValue;
	}

	public void LanguageBtn(){
		GameObject obj_prefab = (GameObject)Resources.Load("Prefab/LanuageWin",typeof(GameObject));
		if(obj_prefab !=null)
		{
			GameObject langWin=(GameObject)Instantiate(obj_prefab);
			langWin.GetComponent<LanguageWin>().InitialData();
		}
	}
}

