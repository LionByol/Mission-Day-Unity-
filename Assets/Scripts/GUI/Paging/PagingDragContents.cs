using UnityEngine;
using System.Collections;

public class PagingDragContents : MonoBehaviour {
	
	public PagingDraggablePanel draggablePanel = null;
	
	// Referenced the panel instead of PagingDraggablePanel script.
	[HideInInspector][SerializeField] UIPanel panel;
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake () {				
		// Legacy functionality support for backwards compatibility
		if (panel != null)
		{
			if (draggablePanel == null)
			{
				draggablePanel = panel.GetComponent<PagingDraggablePanel>();

				if (draggablePanel == null)
				{
					draggablePanel = panel.gameObject.AddComponent<PagingDraggablePanel>();
				}
			}
			panel = null;
		}
	}
	
	/// <summary>
	/// Automatically find the draggable panel if possible.
	/// </summary>

	void Start ()
	{
		if (draggablePanel == null)
		{
			draggablePanel = NGUITools.FindInParents<PagingDraggablePanel>(gameObject);
		}
	}

	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>

	void OnPress (bool pressed)
	{
		if (enabled && gameObject.active && draggablePanel != null)
		{
			draggablePanel.Press(pressed);
		}
	}

	/// <summary>
	/// Drag the object along the plane.
	/// </summary>

	void OnDrag (Vector2 delta)
	{
		if (enabled && gameObject.active && draggablePanel != null)
		{
			draggablePanel.Drag(delta);
		}
	}
	
	/// <summary>
	/// Raises the click event.
	/// </summary>
	void OnClick()
	{
		if (enabled && gameObject.active && draggablePanel != null)
		{
			draggablePanel.currentMomentum = Vector3.zero;
		}
	}
}
