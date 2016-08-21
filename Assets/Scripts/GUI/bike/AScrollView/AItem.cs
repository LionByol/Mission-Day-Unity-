using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class AItem : MonoBehaviour {
	public UISprite map;
	public GameObject lockIcon;
	public UILabel level;
	public UISprite levelColor;
	public UISprite[] star;
	public AScrollView parent;

	int id;

	public void InitialData(int id)
	{	
		this.id=id;
		int starV=GameData.GetStar(id);
		if (starV==-1){
			lockIcon.SetActive(true);
			for (int i=0;i<3;i++){
				star[i].gameObject.SetActive(false);
			}
		}
		else{
			lockIcon.SetActive(false);
			for (int i=0;i<starV;i++){
				star[i].spriteName="star";
			}
		}
		map.spriteName="insignia"+id.ToString();
		level.text=string.Format("{0:00}",id);
	}

	public void SelectMap()
	{
		GameData.selectedLevel=id;
		PlayerPrefs.SetInt("selectedLevel",GameData.selectedLevel);
		parent.glowSprite.transform.position=new Vector3(transform.position.x-0.005f,transform.position.y+0.01f,0);
		parent.ChangeLevel();
	}
}
