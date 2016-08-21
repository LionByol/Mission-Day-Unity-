using UnityEngine;
using System.Collections;

public class PickingGeneralItem : PagingStorageSlot {
	
	public UISprite Faceid;
	public UISprite lockSprite;
	public UILabel PrettyNameCol = null;
	public UILabel PrettyLevelCol = null;

	string mArmyLevelOrNum;
	string mPrettyHealth;
	
	SelectHeroWin mBaizPanel = null;
	//PickingGeneralWin mPickPanel = null;
	
	int idForRoad = -1;
	
	int m_id = -1;
	bool hasChanged = false;
	
	override public int gid 
	{
		get {
			return m_id;
		}
		
		set {
			
			if (m_id != value)
			{
				m_id = value;
				hasChanged = true;
			}
		}
	}
	
	override public void ResetItem()
	{
		hasChanged = true;
	}
	
	// Use this for initialization
	void Start () {
			
		if (mBaizPanel == null)
		{
			mBaizPanel = NGUITools.FindInParents<SelectHeroWin>(gameObject);
		}
	}

	void LateUpdate()
	{
		if (hasChanged == true)
		{
			hasChanged = false;
			
			PickingGeneral item = PickingGeneralManager.instance.GetItem(m_id);
			ApplyGeneralItem(item);
		}
	}


	public void OnItemSelect()
	{
		if (mBaizPanel != null)
		{
			if (m_id>GameManager.TankLevel)
				return;
			mBaizPanel.glowSprite.transform.position=this.transform.position;
			mBaizPanel.glowSprite.transform.localPosition=new Vector3(mBaizPanel.glowSprite.transform.localPosition.x,mBaizPanel.glowSprite.transform.localPosition.y-70f,0);
			mBaizPanel.glowSprite.transform.parent=this.transform;
			mBaizPanel.glowSprite.GetComponent<TweenScale>().ResetToBeginning();
			mBaizPanel.glowSprite.GetComponent<TweenScale>().Play();
			GameManager.instance.curLevel =m_id;
			GameManager.mapName=PrettyNameCol.text;
		}
	}

	public void ApplyGeneralItem(PickingGeneral item)
	{
		if (item==null){
			this.gameObject.SetActive(false);
			return;
		}
		if (Faceid != null)
		{
			Faceid.spriteName = "map"+item.nLevel.ToString();  //map Image
			//Faceid.width=142;
			//Faceid.height=100;
			//Faceid.MakePixelPerfect();
		}
		
		if (PrettyNameCol != null)
		{
			PrettyNameCol.text = item.name;  //  map Name
		}
		if (PrettyLevelCol != null)
		{
			PrettyLevelCol.text = "Level "+item.nLevel.ToString();
		}
		if (GameManager.TankLevel>=item.nLevel){
			lockSprite.gameObject.SetActive(false);
		}
	}
}
