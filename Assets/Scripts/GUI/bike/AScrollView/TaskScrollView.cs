using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TaskScrollView : MonoBehaviour {
	public GameObject item;
	public UIGrid grid;

	public UILabel level;
	public UILabel levelName;
	public UILabel note;
	UIScrollView scroll;

	void Start(){
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
			obj.GetComponent<TaskItem>().InitialData(id);
			if (id==GameData.selectedLevel){
				print (id);
				grid.Reposition();	
				obj.GetComponent<TaskItem>().SelectMap();
			}
		}
		grid.Reposition();	
	}

	public void ChangeLevel(){
	}
}
