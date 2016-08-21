using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	
	public UILabel textPlayerPoints;
	public UILabel textPlayerCoins;
	public UILabel level;
	public AudioSource audioMusic;
	public UISprite exp;
	static public MainMenu instance;
	
	IEnumerator Start () {		
		while (GameManager.instance == null )
			yield return null;
		
		textPlayerPoints.text = GameManager.instance.prefHighScore.ToString("N0"); //"Points: " + Player.instance.points;
		textPlayerCoins.text = GameManager.instance.prefCoins.ToString(); //"Coins: " + Player.instance.coins;
		level.text="LVL."+GameManager.TankLevel.ToString(); 
		SoundManager.AddMusic(audioMusic);
		SoundManager.PlayMusic(audioMusic);
		instance=this;
		SetExp();
	}

	public void SetExp(){
		exp.fillAmount=GameManager.exp/GameManager.maxExp;
	}
}
