using UnityEngine;
using System.Collections;

public class SpringScale : MonoBehaviour {

	//A simple script that scales up gameobject from zero to the original scale
	//Consider using a tween plugin like iTween to improve the behaviours
	//This script is just a simple example to add some pizzazz to the menu items and buttons
	public float _pulseSpeed = 10f;
	public float _pulseAmplitude = .01f; 
	public bool _scaleUpAnim ;
	public float _scaleUpSpeed = .45f;
	public float _scaleUpDelay = 0f;

	private bool _scaleUp ;    
	private Vector3 _scaleSave ;
	private bool _pulse ;
	private Vector3 _velocity = Vector3.zero;
		
	IEnumerator Start () {
		if(_scaleUpAnim){
		_scaleSave = transform.localScale;
		transform.localScale = Vector3.zero;
		yield return new WaitForSeconds(_scaleUpDelay);
		_scaleUp = true;
		}
	}

	void LateUpdate () {
		if(_scaleUp && !_pulse && transform.localScale.x+.01 < _scaleSave.x){		
			transform.localScale = Vector3.SmoothDamp(transform.localScale, _scaleSave ,ref _velocity, _scaleUpSpeed);
		}else if(_scaleUp && _pulseSpeed > 0 && _pulseAmplitude > 0){	
			_pulse = true;
	  	 	float tem= _pulseAmplitude * Mathf.Sin(Time.time * _pulseSpeed);
			transform.localScale=new Vector3(tem,transform.localScale.y+tem,tem);
	   	}
	}
}