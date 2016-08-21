using UnityEngine;
using System.Collections;

public class PagingSurface : MonoBehaviour {
	
	public GameObject template = null;
	PagingStorageSlot[] mItemSlots;
	bool hasChanged = false;
	int mPagedID = -1;
	
	public float cellWidth = 70;
	public float cellHeight = 90; 
	public int Rows = 0;
	public int columns = 0;
	
	// Use this for initialization
	void Awake () {
				
		if (template != null)
		{
			float depth = gameObject.transform.localPosition.z;
			
			int SLOTS_NUM = Rows*columns;
			mItemSlots = new PagingStorageSlot[SLOTS_NUM];
			for (int i=0; i<Rows; ++ i)
			{
				for (int j=0; j<columns; ++ j)
				{
					int itemID = i*columns + j;
					GameObject go = NGUITools.AddChild(gameObject, template);
					Transform t = go.transform;
					t.localPosition = new Vector3((j+0.5f) * cellWidth, - (i+0.5f) * cellHeight, depth);
					
					PagingStorageSlot ts = go.GetComponent<PagingStorageSlot>();
					mItemSlots[itemID] = ts;
				}
			}
		}
	}

	public int gid
	{
		get
		{
			return mPagedID;
		}
		
		set 
		{
			if (mPagedID != value)
			{
				mPagedID = value;
				hasChanged = true;
			}
		}
	}
	
	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void ResetSurface()
	{
		int numPerPaged = Rows*columns;
		int imax = Rows*columns;
		for (int i=0; i<imax; ++ i)
		{
			PagingStorageSlot t = mItemSlots[i];
			t.gid = (mPagedID) * numPerPaged + i;
			t.ResetItem();
		}
	}
	
	/// <summary>
	/// Lates the update.
	/// </summary>
	
	void LateUpdate() {
		
		if (true == hasChanged)
		{	
			hasChanged = false;
			
			int numPerPaged = Rows*columns;
			for (int i=0; i<numPerPaged; ++ i)
			{
				PagingStorageSlot go = mItemSlots[i];
				go.gid = (mPagedID) * numPerPaged + i;
			}
		}
	}
}
