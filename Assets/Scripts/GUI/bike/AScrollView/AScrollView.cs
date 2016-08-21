using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class AScrollView : MonoBehaviour {
	public GameObject item;
	public UIGrid grid;
	public GameObject glowSprite;

	public UILabel levelName;

	public UILabel TaskStone;
    public UILabel TaskUfo;
	public UISprite map;
	public GameObject lockIcon;
	public UISprite[] star;
	UIScrollView scroll;

	void Start(){
        GameData.selectedLevel = PlayerPrefs.GetInt("selectedLevel", 1);
		scroll=grid.transform.parent.GetComponent<UIScrollView>();
		InitialData();
		ChangeLevel();
	}

	public void InitialData()
	{
		for (int id=1;id<GameData.MaxLevel+1;id++)
		{
			GameObject obj = NGUITools.AddChild(grid.gameObject,item);
			obj.name = "map"+id.ToString();
			obj.GetComponent<AItem>().InitialData(id);
			if (id==GameData.selectedLevel){
				print (id);
				grid.Reposition();	
				obj.GetComponent<AItem>().SelectMap();
			}
		}
		grid.Reposition();	
	}

	public void OnRightArrow(){
//		scroll.MoveRelative(new Vector3(680f,0));
		scroll.Scroll(0.5f);
//		scroll.RestrictWithinBounds(false, scroll.canMoveHorizontally, scroll.canMoveVertically);
	}

	public void OnLeftArrow(){
//		scroll.MoveRelative(new Vector3(-680f,0));
		scroll.Scroll(-0.5f);
//		scroll.RestrictWithinBounds(false, scroll.canMoveHorizontally, scroll.canMoveVertically);
	}

	public void ChangeLevel(){
		//level.text="Stage."+string.Format("{0:00}",GameData.selectedLevel);

		LevelData levelData=(LevelData)(LoadData.LevelList[GameData.selectedLevel]);

        TaskStone.text = levelData.greyStone.ToString() + " of stones to destroy";

        int _blueUFOAmount = (GameData.selectedLevel - 1) / 2;
        int _redUFOAmount = (GameData.selectedLevel - 1) / 3;
        int _greenUFOAmount = (GameData.selectedLevel - 1) / 4;
        int ufoCnt = _blueUFOAmount + _redUFOAmount + _greenUFOAmount;

        TaskUfo.text = ufoCnt.ToString() + " of UFO to destroy";
        levelName.text = "Your rank:    " + Language.Get("Level" + GameData.selectedLevel.ToString());

		//update Main Map UI
		int starV=GameData.GetStar(GameData.selectedLevel);
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
		map.spriteName="insignia"+(GameData.selectedLevel).ToString();

	}
}
