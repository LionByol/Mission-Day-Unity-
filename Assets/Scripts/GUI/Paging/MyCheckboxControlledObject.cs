using UnityEngine;
using System.Collections;

public class MyCheckboxControlledObject : MonoBehaviour
{ 
	public GameObject target;
	public bool inverse = false;
	public GameObject EventReceiver;
	public string 	FunctionName;
	public bool isStart = false;
	void OnActivate (bool isActive)
	{
		if (target != null) NGUITools.SetActive(target, inverse ? !isActive : isActive);
		bool CallEvent = inverse ? !isActive : isActive;
		
		if(isStart && EventReceiver != null && !string.IsNullOrEmpty(FunctionName) && CallEvent)
		{
			EventReceiver.SendMessage(FunctionName);
		}
	}
	void Update()
	{
		isStart = true;
	}
}
