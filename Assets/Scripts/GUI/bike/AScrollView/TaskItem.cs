using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TaskItem : MonoBehaviour {
	public UISprite map;
	public UILabel level;
	public UISprite levelColor;
	public UISprite[] star;
	public TaskScrollView parent;

	int id;

	public void InitialData(int id)
	{	
		this.id=id;
		map.spriteName="sd_0"+(id%7).ToString()+"_b";
		level.text=string.Format("{0:00}",id);
	}

	public void SelectMap()
	{

	}
}
