using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickingGeneral
{
	public string name;
	public int nLevel = 0;
	public bool campIn = false;
}

public class PickingGeneralManager : MonoBehaviour {
	
	static PickingGeneralManager mInst = null;
	
	List<PickingGeneral> mGenerals = new List<PickingGeneral>();

	/// <summary>
	/// The instance of the CombatManager class. Will create it if one isn't already around.
	/// </summary>

	static public PickingGeneralManager instance
	{
		get
		{
			if (mInst == null)
			{
				mInst = Object.FindObjectOfType(typeof(PickingGeneralManager)) as PickingGeneralManager;

				if (mInst == null)
				{
					GameObject go = new GameObject("_PickingGeneralManager");
					DontDestroyOnLoad(go);
					mInst = go.AddComponent<PickingGeneralManager>();
				}
			}
			
			return mInst;
		}
	}

	public void ApplyHireGeneral()
	{
		mGenerals.Clear();
		string[] maplist={"map1","map2","map3","map4","map5","map6","map7","map8","map9","map10","map11"};
		int cn=0;
		foreach(string	mapName in maplist)  
		{
			cn=cn+1;
				PickingGeneral item = new PickingGeneral();
				item.nLevel = cn;				
				item.name = mapName;
				item.campIn = false;
				
				mGenerals.Add(item);		
		}
	}

	public PickingGeneral GetItem(int itemID)
	{
		if (itemID<0 || itemID> (mGenerals.Count-1)) return null;
		return mGenerals[itemID];
	}

	public List<PickingGeneral> GetGeneralList()
	{
		return mGenerals;
	}

}
