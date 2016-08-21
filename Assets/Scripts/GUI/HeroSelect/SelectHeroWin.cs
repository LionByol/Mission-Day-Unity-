using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectHeroWin: MonoBehaviour {
	
	PickingGeneralItem[] camps = new PickingGeneralItem[5];
	
	public delegate void ApplyBaizCampDataDelegate(List<PickingGeneral> data);
	public static ApplyBaizCampDataDelegate applyBaizCampDataDelegate = null;
	public PagingStorage uiGrid = null;
	public GameObject glowSprite;

	enum Method
	{
		Picking = 0,
		ArgsRoad = 1,
	}
	
	Method PickingStyle = Method.Picking;
	
	public float Depth(float depth)
	{
		return transform.localPosition.z;
	}
	

	public void OnClose()
	{
		NGUITools.SetActive(gameObject,false);
		Destroy(gameObject);
	}

	void Start(){
		AssignRoadGeneral();
	}
	public void AssignRoadGeneral()
	{
		PickingStyle = Method.Picking;
		
		if (uiGrid != null)
		{
			PickingGeneralManager.instance.ApplyHireGeneral();
			List<PickingGeneral> cacheList = PickingGeneralManager.instance.GetGeneralList();
			uiGrid.SetCapacity(cacheList.Count);
			uiGrid.ResetAllSurfaces();
			
//			// 如果数量小0 ...
//			if (cacheList.Count <= 0)
//			{
//				if (RoadTipCol != null) 
//				{
//					int Tipset = VariableScript.INSTANCE_PICKING_GENERAL_NO_FREE;
//					RoadTipCol.text = U3dCmn.GetWarnErrTipFromMB(Tipset);
//					RoadTipCol.enabled = true;
//				}
//			}
		}
	}

}
