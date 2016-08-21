using UnityEngine;
using System.Collections;

public class PagingSpringPanel : IgnoreTimeScale {
	
	public Vector3 target = Vector3.zero;
	public float strength = 10f;

	Vector3 mLastPos = Vector3.zero;
	float mThreshold = 0.5f;
	PagingDraggablePanel mDrag;
	
	// Use this for initialization
	void Start () {
		mDrag = GetComponent<PagingDraggablePanel>();
	}
	
	// Update is called once per frame
	void Update () {
			
		float delta = UpdateRealTimeDelta();
		Vector3 before = mLastPos;
		
		Vector3 t = NGUIMath.SpringLerp(mLastPos, target, strength, delta);
		mLastPos = t;
		
		Vector3 relative = mLastPos - before;
		if (relative.magnitude > 0.05f)
		{
			mDrag.MoveRelative(relative);
			mDrag.SpringDrag();
		}

		if (mThreshold >= (target - mLastPos).magnitude) 
		{
			enabled = false;
		}
	}
	
	/// <summary>
	/// Start the tweening process.
	/// </summary>

	static public PagingSpringPanel Begin (GameObject go, Vector3 pos, float strength)
	{
		PagingSpringPanel sp = go.GetComponent<PagingSpringPanel>();
		if (sp == null) sp = go.AddComponent<PagingSpringPanel>();
		sp.target = pos;
		sp.strength = strength;
		sp.mLastPos = Vector3.zero;
		sp.mThreshold = 0.5f;

		if (!sp.enabled)
		{
			sp.enabled = true;
		}
		return sp;
	}

}
