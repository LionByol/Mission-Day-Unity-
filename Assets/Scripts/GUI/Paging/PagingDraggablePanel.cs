using UnityEngine;
using System.Collections;

public class PagingDraggablePanel : IgnoreTimeScale {
	
	public enum DragEffect
	{
		None,
		MomentumAndSpring,
	}
	
	/// <summary>
	///  Effect to apply when dragging. 
	/// </summary> 

	DragEffect dragEffect = DragEffect.MomentumAndSpring;
	
	/// <summary>
	/// Whether dragging will be disabled if the contents fit.
	/// </summary>

	public bool disableDragIfFits = false;
		
	/// <summary>
	/// How much momentum gets applied when the press is released after dragging.
	/// </summary>
	
	public PagingStorage uiPaging = null;
	public float momentumAmount = 35f;
	public float momentumStrength = 4.5f;
	public Vector3 scale = Vector3.one;
	
	UIPanel mPanel;
	Vector3 mLastPos;
	Vector3 mBasedOnDelta = Vector3.zero;
	Transform mTrans;
	Plane mPlane;

	bool mPressed = false;
	bool mShouldMove = false;
	bool mCalculatedBounds = false;
	
	Vector3 mMomentum = Vector3.zero;
	Bounds mBounds;
	float mScroll = 0f;
	int mTouches = 0;
	
	/// <summary>
	/// Current momentum, exposed just in case it's needed.
	/// </summary>

	public Vector3 currentMomentum { get { return mMomentum; } set { mMomentum = value; } }
	
	/// <summary>
	/// Gets the storage.
	/// </summary>
	public PagingStorage Storage { get { return uiPaging; } }
	
	/// <summary>
	///  Calculate the bounds used by the widgets.
	/// </summary>

	public Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
			}
			
			return mBounds;
		}
	}
	
	/// <summary>
	/// Whether the contents of the panel should actually be draggable depends on whether they currently fit or not.
	/// </summary>

	bool shouldMove
	{
		get
		{
			if (!disableDragIfFits) return true;
			
			if (mPanel == null) mPanel = GetComponent<UIPanel>();
			Vector4 clip = mPanel.clipRange;
			Bounds b = bounds;

			float hx = clip.z * 0.5f;
			float hy = clip.w * 0.5f;

			if (!Mathf.Approximately(scale.x, 0f))
			{
				if (b.min.x < clip.x - hx) return true;
				if (b.max.x > clip.x + hx) return true;
			}

			if (!Mathf.Approximately(scale.y, 0f))
			{
				if (b.min.y < clip.y - hy) return true;
				if (b.max.y > clip.y + hy) return true;
			}
			return false;
		}
	}
	
	void Awake ()
	{
		mTrans = transform;
		mPanel = GetComponent<UIPanel>();
	}
	// Use this for initialization
	void Start () { 

	}
	

	public void RestrictWithinBounds(bool instant, Bounds b)
	{
		Vector3 constraint = mPanel.CalculateConstrainOffset(b.min, b.max);
		if (constraint.magnitude > 0.005f)
		{
			if (!instant && dragEffect == DragEffect.MomentumAndSpring)
			{
				// Spring back into place
				constraint.Scale(scale);
				PagingSpringPanel.Begin(mPanel.gameObject, constraint, 13f);
			}
			else
			{
				// Jump back into place
				MoveRelative(constraint);
			}
			
			mMomentum = Vector3.zero;
			mScroll = 0f;
		}
		else
		{
			// Remove the spring as it's no longer needed
			DisableSpring();
		}
	}
	
	/// <summary>
	/// Disable spring panel
	/// </summary>
	void DisableSpring()
	{
		PagingSpringPanel sp = GetComponent<PagingSpringPanel>();
		if (sp != null) sp.enabled = false;
	}
	
	/// <summary>
	/// Move the panel by the specified amount
	/// </summary>
	public void MoveRelative (Vector3 relative)
	{
		if (enabled && gameObject.active && uiPaging != null)
		{
			mBasedOnDelta += relative;
			
			float x = Mathf.Floor(mBasedOnDelta.x);
			float y = Mathf.Floor(mBasedOnDelta.y);
			Vector3 delta = new Vector3(x, y, 0f);
			
			if (delta.magnitude > 0.005f)
			{
				// Move child transform
				uiPaging.Drag(delta);
				mCalculatedBounds = false;
			}
			
			mBasedOnDelta -= delta;
		}
	}
	
	void MoveAbsolute (Vector3 absolute)
	{
		Vector3 a = mTrans.InverseTransformPoint(absolute);
		Vector3 b = mTrans.InverseTransformPoint(Vector3.zero);
		MoveRelative(a - b);
	}
	
	public void SpringDrag ()
	{
		if (enabled && gameObject.active && uiPaging != null)
		{
			uiPaging.MoveDrag( mPanel.clipRange);
		}
	}

	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>

	public void Press (bool pressed)
	{
		if (enabled && gameObject.active)
		{
			mTouches += (pressed ? 1: -1);
			mCalculatedBounds = false;
			
			bool disDragIfNoPaged = false; 
			if (uiPaging != null)  
			{ disDragIfNoPaged = uiPaging.disableDragIfNoPaged; }
			
			if (true == disDragIfNoPaged) return;
			mShouldMove = shouldMove;
			if (!mShouldMove) return;
			mPressed = pressed;
			
			if (pressed)
			{
				// Remove all momentum on press
				mMomentum = Vector3.zero;
				
				// Disable the spring movement
				DisableSpring();
				
				// Remember the last hit position
				mLastPos = UICamera.lastHit.point;
				
				// Create the plane to drag along
				mPlane = new Plane(mTrans.rotation * Vector3.back, mLastPos);
			}
			else if (dragEffect == DragEffect.MomentumAndSpring)
			{
				if (uiPaging != null)
				{
					uiPaging.DragCalmPagedScroll();
					Bounds b = uiPaging.GetPagedBounds(mTrans);
					RestrictWithinBounds(false, b);
				}
			}
		}
	}
	
	/// <summary>
	/// Drag event receiver.
	/// </summary>

	public void Drag (Vector2 delta)
	{
		if (enabled && gameObject.active && mShouldMove)
		{
			// If drag delta magnitude is less than 9f, no clicks
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float dist = 0f;
			
			if (mPlane.Raycast(ray, out dist))
			{
				Vector3 currentPos = ray.GetPoint(dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;
			
				if (offset.x != 0f || offset.y != 0f)
				{
					offset = mTrans.InverseTransformDirection(offset);
					offset.Scale(scale);
					offset = mTrans.TransformDirection(offset);
				}
				
				// Restrict within in panel
				Vector3 constraint = mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
				if (mPanel.clipping != UIDrawCall.Clipping.None && dragEffect == DragEffect.MomentumAndSpring)
				{
					if (constraint.magnitude > 0.005f)
					{
						Vector3 clipScale = Vector3.one;
						float hx = mPanel.clipRange.z * 0.67f;
						float hy = mPanel.clipRange.w * 0.67f;
						clipScale.x = 1f - Mathf.Clamp01(Mathf.Abs(constraint.x) / hx);
						clipScale.y = 1f - Mathf.Clamp01(Mathf.Abs(constraint.y) / hy);
						offset.Scale(clipScale);
					}
				}
		
				// Adjust the momentum
				mMomentum = Vector3.Lerp(mMomentum, mMomentum + offset * (0.01f*momentumAmount), 0.67f);
				
				// Move the panel
				MoveAbsolute(offset);
				
				// We want to constrain the UI to be within bounds
				if (uiPaging != null)
				{
					uiPaging.MoveDrag(mPanel.clipRange);
				}
			}
		}
		

	}
	
	public void RestrictVisibleWithinBounds()
	{
		if (uiPaging != null)
		{
			Bounds b = uiPaging.GetPagedBounds(mTrans);
			RestrictWithinBounds(false, b);
		}
	}
	
	/// <summary>
	/// Lates the update.
	/// </summary>
	public void LateUpdate ()
	{
#if UNITY_EDITOR
		// Don't play animations when not in play mode
		if (!Application.isPlaying)
			return;
#endif
		
		float delta = UpdateRealTimeDelta();

		// Apply the momentum
		if (mShouldMove && !mPressed)
		{
			if (mMomentum.magnitude > 0.005f)
			{				
				// 9f;
				// Move the panel
				Vector3 offset = NGUIMath.SpringDampen(ref mMomentum, momentumStrength, delta);
				MoveAbsolute(offset);
				
				// Restrict the contents to be within the panel's bounds
				if (mPanel.clipping != UIDrawCall.Clipping.None) RestrictWithinBounds(false, bounds);
				
				if (uiPaging != null)
				{
					uiPaging.MoveDrag(mPanel.clipRange);
				}
				
				return;
			}
			else mScroll = 0f;
		}
		else mScroll = 0f;
		
		// Dampen the momentum
		NGUIMath.SpringDampen(ref mMomentum, 9f, delta);
	}
	
#if UNITY_EDITOR

	/// <summary>
	/// Draw a visible orange outline of the bounds.
	/// </summary>

	void OnDrawGizmos ()
	{
		if (mPanel != null)// && mPanel.debugInfo == UIPanel.DebugInfo.Gizmos)
		{
			Bounds b = bounds;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 0.4f, 0f);
			Gizmos.DrawWireCube(new Vector3(b.center.x, b.center.y, b.min.z), new Vector3(b.size.x, b.size.y, 0f));
		}
	}
#endif
}
