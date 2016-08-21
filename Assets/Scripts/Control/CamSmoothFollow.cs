/*
This camera smoothes out rotation around the y-axis and height.
Horizontal Distance to the target is always fixed.

There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

For every of those smoothed values we calculate the wanted value and the current value.
Then we smooth it using the Lerp function.
Then we apply the smoothed values to the transform's position.
*/
using UnityEngine;
using System.Collections;

public class CamSmoothFollow : MonoBehaviour
{
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float targetdistance;
	public float distance = 6.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	// Look above the target (height * this ratio)
	public float targetHeightRatio = 0.5f;
	// How fast we reach the target values
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public bool followVelocity = false;
	public float velocityDamping = 5.0f;
	private Vector3 lastPos = Vector3.zero;
	private Vector3 currentVelocity = Vector3.zero;
	private float wantedRotationAngle = 0.0f;
	[HideInInspector]
	public bool reset = true;		// Make true from scripting for resetting the direction settings
	
	private Transform tr;

	public static CamSmoothFollow instance;
	void Awake()
	{	
		instance=this;
		tr = transform;
	}

	void LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target)
			return;
	
		if (reset) {
			lastPos = target.position;
			wantedRotationAngle = target.eulerAngles.y;
			currentVelocity = target.forward * 2.0f;
			reset = false;
		}
	
		Vector3 updatedVelocity = (target.position - lastPos)/Time.deltaTime;
		updatedVelocity.y = 0.0f;
	
	
		if (updatedVelocity.magnitude>1.0f) {
			currentVelocity = Vector3.Lerp (currentVelocity, updatedVelocity, velocityDamping * Time.deltaTime);
			wantedRotationAngle = Mathf.Atan2(currentVelocity.x, currentVelocity.z) * Mathf.Rad2Deg;
		}
		lastPos = target.position;
	
		if (!followVelocity)
			wantedRotationAngle = target.eulerAngles.y;

		float wantedHeight = target.position.y + height;

		float currentRotationAngle = tr.eulerAngles.y;
		float currentHeight = tr.position.y;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0,currentRotationAngle,0);
		
		tr.position = Vector3.Lerp(tr.position, target.position, Time.deltaTime*100.0f);

		distance = Mathf.Lerp(distance, targetdistance, Time.deltaTime);

		tr.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		Vector3 t = tr.position;
		t.y = currentHeight;
		tr.position = t;
		/**if (targetRigidBody) {
			// We use centerOfMass instead of worldCenterOfMass because the first one is interpolated.
			Vector3 localScale = target.localScale;
			Vector3 CoM = Vector3.Scale (targetRigidBody.centerOfMass, new  Vector3 (1.0f /localScale.x, 1.0f /localScale.y, 1.0f /localScale.z));
			CoM = target.TransformPoint(CoM);
			tr.LookAt(CoM + Vector3.up *height *targetHeightRatio);
		} else
		{**/
			tr.LookAt(target.position + Vector3.up*height *targetHeightRatio);
		//}
	}
}
