using UnityEngine;
using System.Collections.Generic;

public class PagingStorage : MonoBehaviour {
		
	public enum Arrangement
	{
		Horizontal,
		Vertical,
	}

	Arrangement arrangement = Arrangement.Horizontal;
	
	Vector3 mBasedOnDelta = Vector3.zero;

	public int spacing = 210;
	bool hideInactive = false;

	int SURFACE_NUM = 0;
	PagingSurface[] mPagedSlots;
	
	int pagedCarX = 0;
	int pagedOnScroll = 0;
	
	int PAGING_NUM = 5;
	int mArrangeCapacity = 10;
	
	public int pagedDragID = 0;
	
	Transform mTrans;
	
	
	public delegate void DragPageDelegate(int nPageID);
	public DragPageDelegate onDragingPage = null;
	
	void Awake() {
		mTrans = transform;
		Reposition();
	}
	
	// Use this for initialization
	void Start () {

	}
	
	/// <summary>
	/// Sets the capacity.
	/// </summary>
	/// <param name='maxCount'>
	/// Max count.
	/// </param>
	public void SetCapacity(int maxCount)
	{
		mArrangeCapacity = maxCount;
				
		PagingSurface t0 = mPagedSlots[0];
		if (t0 == null) return;
		
		int numPerPaged = t0.columns*t0.Rows;
		int nPaged = mArrangeCapacity/numPerPaged;
		int ox = mArrangeCapacity - nPaged*numPerPaged;
		if (ox>0) nPaged ++;
		
		PAGING_NUM = nPaged;
		
		// Hide no use surface
		int imax = Mathf.Min(SURFACE_NUM,PAGING_NUM);
		
		int i = 0;
		for (; i<imax; ++ i)
		{
			PagingSurface s = mPagedSlots[i];
			if (false == s.gameObject.active) {
				s.gameObject.SetActiveRecursively(true);
			}
		}
		
		for (; i<SURFACE_NUM; ++ i)
		{
			PagingSurface s = mPagedSlots[i];
			s.gameObject.SetActiveRecursively(false);
		}
	}
	
	/// <summary>
	/// Gets the items.
	/// </summary>
	/// <returns>
	/// The items.
	/// </returns>
	public List<GameObject> GetAvailableItems()
	{
		List<GameObject> itemList = new List<GameObject>();
		
		int imax = Mathf.Min(PAGING_NUM, SURFACE_NUM);
		for (int i=0; i<imax; ++ i)
		{
			PagingSurface ts = mPagedSlots[i];
		
			Transform myTrans = ts.transform;
			int childCount = myTrans.childCount;
			if (childCount == 0) continue;
			
			for (int j=0; j<childCount; ++ j)
			{
				Transform child = myTrans.GetChild(j);
				itemList.Add(child.gameObject);
			}
		}
		
		return itemList;
	}
	
	/// <summary>
	/// Resets all surfaces.
	/// </summary>
	public void ResetAllSurfaces()
	{		
		ResetAllSurfaces1(pagedDragID);
	}
	
	public void ResetAllSurfaces1(int cc)
	{		
		mBasedOnDelta = Vector3.zero;
		int X = Mathf.Min(PAGING_NUM-1,cc);
			X = Mathf.Max(0,X);
		
		int SURFACE_NUM_1 = Mathf.Min(PAGING_NUM,SURFACE_NUM);
		if (SURFACE_NUM_1 < 1) 
		{
			return;
		}
		
		int[] prevLine = new int[SURFACE_NUM_1];
		int MidCol = SURFACE_NUM_1 / 2;
		int startLine = MidCol;
		
		if (X>=0 && X<MidCol)
		{
			startLine = X;
		}
		else if (X>=(PAGING_NUM-MidCol) && X<PAGING_NUM)
		{
			startLine = SURFACE_NUM_1 + X - PAGING_NUM;
		}
		
		int c=0;
		for(int i=startLine; i>0; -- i, ++ c)
		{
			prevLine[c] = X-i;
		}
		
		int c1 = c;
		for (int i=0;c<SURFACE_NUM_1; ++ c, ++ i)
		{
			prevLine[c] = X+i;
		}
		
		// 重新布置数据页 .... 
		int tocX = pagedCarX;
		for (int j=0; j<SURFACE_NUM_1; ++ j)
		{
			tocX = (SURFACE_NUM_1 + pagedCarX + j) % SURFACE_NUM_1;
			PagingSurface go = mPagedSlots[tocX];
			go.gid = prevLine[j];
			go.ResetSurface();
		}
		
		tocX = (SURFACE_NUM_1 + pagedCarX + c1) % SURFACE_NUM_1;
		PagingSurface center = mPagedSlots[tocX];
		this.pagedDragID = center.gid;
		this.pagedOnScroll = tocX;
		
		if (onDragingPage != null) {
			onDragingPage(pagedDragID);
		}
	}
	
	/// <summary>
	/// Gets the paged bounds.
	/// </summary>
	public Bounds GetPagedBounds(Transform root)
	{
		// 稳定页 ...
		int imax = Mathf.Min(PAGING_NUM, SURFACE_NUM);
		int onScroll = Mathf.Min(imax-1, pagedOnScroll);
			onScroll = Mathf.Max(0,onScroll);
		
		Transform child = mTrans.GetChild(onScroll);
		return NGUIMath.CalculateRelativeWidgetBounds(root,child);
	}
	
	public void MoveDrag(Vector4 clipRange)
	{
		int scroll = 0;
		
		float spScroll = spacing * 1f;
		if (Mathf.Abs(mBasedOnDelta.x) > spScroll)
		{
			scroll = (int) (mBasedOnDelta.x/spScroll);
			mBasedOnDelta.x -= (scroll*spScroll);
		}
		
		Vector3 spCarX = new Vector3(spacing,0f,0f);
		Vector3 spCarY = new Vector3(0f,spacing,0f);
				
		int SURFACE_NUM_1 = Mathf.Min(PAGING_NUM,SURFACE_NUM);
		if (SURFACE_NUM_1 < 1) 
		{
			return;
		}
		
		int toCarX = (SURFACE_NUM_1 + pagedCarX - 1) % SURFACE_NUM_1;
		
		float hx = clipRange.z * 0.5f;
		float hy = clipRange.w * 0.5f;
		float twoSpacing = 2.0f * spacing;
		
		if (scroll<0)
		{
			// Move left to right
			int num = Mathf.Abs(scroll);
			int toc = pagedCarX;
			
			PagingSurface t = mPagedSlots[pagedCarX];
			PagingSurface t1 = mPagedSlots[toCarX];

			if ((t1.gid < (PAGING_NUM-1)) && (t.gameObject.transform.localPosition.x + hx < -twoSpacing))
			{
				int ox1 = PAGING_NUM-t1.gid-1;
				if (ox1<num) num = ox1;
				t = mPagedSlots[toCarX];
				for (int i=0; i<num; ++ i)
				{
					toc = (SURFACE_NUM_1 + pagedCarX + i) % SURFACE_NUM_1;
					PagingSurface ts = mPagedSlots[toc];
					ts.gid =  t.gid + 1;
					
					ts.gameObject.transform.localPosition = t.gameObject.transform.localPosition + spCarX;
					t = ts;
				}
		
				pagedCarX = (SURFACE_NUM_1 + toc + 1) % SURFACE_NUM_1;
			}
			else 
			{
				mBasedOnDelta = Vector3.zero;
			}
		}
		else if (scroll>0)
		{
			// Move right to left
			int num = Mathf.Abs(scroll);
			int toc = toCarX;
			
			PagingSurface t = mPagedSlots[toCarX];
			PagingSurface t0 = mPagedSlots[pagedCarX];
			if ((t0.gid>0) && (t.gameObject.transform.localPosition.x - hx > twoSpacing))
			{	
				if (t0.gid<num) num = t0.gid;
				t = mPagedSlots[pagedCarX];
				for (int i=0; i<num; ++ i)
				{
					toc = (SURFACE_NUM_1 + toCarX - i) % SURFACE_NUM_1;
					PagingSurface ts = mPagedSlots[toc];
					ts.gid = t.gid - 1;
					
					ts.gameObject.transform.localPosition = t.gameObject.transform.localPosition - spCarX;
					t = ts;
				}
				
				pagedCarX = toc;
			}
			else 
			{
				mBasedOnDelta = Vector3.zero;
			}
		}
	}
	
	public void DragCalmPagedScroll()
	{
		float spScroll = spacing * 0.25f;
		float spScroll_1 = spacing * 0.5f;
		
		int SURFACE_NUM_1 = Mathf.Min(PAGING_NUM, SURFACE_NUM);
		if (SURFACE_NUM_1 < 1)
		{
			return;
		}
		
		// 断定当前页是否在滑动中 ...
		int paged1 = pagedOnScroll;
		Transform c1 = mTrans.GetChild(paged1);
		PagingSurface ps1 = c1.gameObject.GetComponent<PagingSurface>();
		float d1 = Vector3.Distance(c1.localPosition, Vector3.zero);
		if (d1 > spScroll && d1 < spScroll_1)
		{
			float sp1 = c1.localPosition.x;
			int pagedID = ps1.gid;
			
			if (sp1 < 0) 
			{
				if ((PAGING_NUM-1) > pagedID)
				{
					pagedOnScroll = (SURFACE_NUM_1 + pagedOnScroll + 1) % SURFACE_NUM_1;
				}
			}
			else 
			{
				if ( pagedID > 0)
				{
					pagedOnScroll = (SURFACE_NUM_1 + pagedOnScroll - 1) % SURFACE_NUM_1;
				}
			}
			
			c1 = mTrans.GetChild(pagedOnScroll);
			ps1 = c1.gameObject.GetComponent<PagingSurface>();
			pagedDragID = ps1.gid;
			
			// 定当前页 ...
			if (this.onDragingPage != null) {
				this.onDragingPage(pagedDragID);
			}
			
			return;
		}
		
		// pagedOnScroll
		for (int i=0; i<SURFACE_NUM_1; ++ i)
		{
			int paged = (SURFACE_NUM_1 + pagedCarX + i) % SURFACE_NUM_1;
			Transform child = mTrans.GetChild(paged);
			
			float dist = Vector3.Distance(child.localPosition, Vector3.zero);
			if (dist < spScroll_1)
			{		
				pagedOnScroll = Mathf.Min(SURFACE_NUM_1-1, paged);
				pagedOnScroll = Mathf.Max(0,pagedOnScroll);
				
				c1 = mTrans.GetChild(pagedOnScroll);
				ps1 = c1.gameObject.GetComponent<PagingSurface>();
				pagedDragID = ps1.gid;
				
				// 定当前页 ...
				if (this.onDragingPage != null) {
					this.onDragingPage(pagedDragID);
				}
				
				return;
			}
		}
	}
	
	/// <summary>
	/// Drag the specified delta and inSpring.
	/// </summary>
	public void Drag(Vector3 delta)
	{
		// inSpring , for spring move
		mBasedOnDelta += delta;
		
		foreach (Transform t in mTrans)
		{
			t.localPosition += delta;
		}
	}
	
	/// <summary>
	/// Recalculate the position of all elements within the table, sorting them alphabetically if necessary.
	/// </summary>

	void Reposition ()
	{
		Transform myTrans = transform;
		List<Transform> children = new List<Transform>();
		
		for (int i=0, imax=myTrans.childCount; i < imax; ++i)
		{
			Transform child = myTrans.GetChild(i);
			if (child && (!hideInactive || child.gameObject.active)) children.Add(child);
		}
		
		if (children.Count == 0) return;
		mArrangeCapacity = 0;
				
		SURFACE_NUM = children.Count;
		mPagedSlots  = new PagingSurface[SURFACE_NUM];
		for (int i = 0, imax = SURFACE_NUM; i < imax; ++i)
		{
			Transform t = children[i];
			float depth = t.localPosition.z;
			t.localPosition = (arrangement == Arrangement.Horizontal) ?
				new Vector3(spacing * i, 0, depth) :
				new Vector3(0f, -spacing * i, depth);
			
			PagingSurface ts = t.gameObject.GetComponent<PagingSurface>();
			ts.gid = (i);
			
			// 每页数量 
			int numPerPaged = ts.Rows * ts.columns;
			mArrangeCapacity += numPerPaged; 
			mPagedSlots[i] = ts;
		}
	}
	
	public bool disableDragIfNoPaged 
	{
		get {	
			return (PAGING_NUM <1);
		}
	}
	
}
