using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
		//Button script used by all buttons in the game
	public AudioClip _buttonSound;			//Place sound clip for when button is pressed down
	public AudioClip _buttonSoundUp;		//Place sound clip for when button is released (this can be based on type of button)
	public Mesh _playMesh ;					//Mesh for play button after pause has been pressed
	public GameObject _pauseMenu ;			//GameObject containing the buttons to be visible during pause
	public GameObject _quitMenu;			
	public GameObject _offIcon;			//Place icon for on/off buttons
	public Camera _guiCamera ;				//The gui camera(Ortographic) provides more accurate raycasting for small buttons

	public float _scaleDown  = 0.9f;
	public string _sceneToLoad;			//Enter the name of scene to load when button is pressed
	private Vector3 _saveScale ;		//Saves button scale
	private Mesh _savePauseMesh ;	//Saves pause button mesh


	void Start () {
		_saveScale = transform.localScale;				//Save the scale for reset
		if(SaveStats.instance._saveMusicVol==0){
		SaveStats.instance._saveMusicVol = SoundController.instance._musicVol;
		SaveStats.instance._saveSoundVol = SoundController.instance._soundVol;
		}
		if(!_guiCamera){
			GameObject camGO  = GameObject.Find("_Gui Camera");
			if(camGO){
				_guiCamera = camGO.GetComponent<Camera>();
			}else{
				_guiCamera = Camera.main;
			}	
		}	
	}

	void Update () {
		foreach (Touch touch in Input.touches) {
			RaycastHit hit  = new RaycastHit();
			bool t ;
			Ray ray = _guiCamera.ScreenPointToRay (touch.position);
			if (touch.phase == TouchPhase.Began && Physics.Raycast(ray,out hit,500000)) {	
				if (hit.collider.gameObject == this.gameObject) {						//User touches a button
					OnButtonDown();
					t= true;
				}
			}		
		}
	}
	#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE || UNITY_WEBPLAYER
	void OnMouseUp () {
		transform.localScale = _saveScale;
	}
	#endif

	void OnButtonDown () {
		if(this._buttonSound)
		SoundController.instance.Play(_buttonSound, 2, Random.Range(1.5f,1.25f));	//Play a sound when pressing button
		transform.localScale = _saveScale * _scaleDown;							//Scale the button down to indicate button is being pressed
	}

	void OnMouseDown () {
		OnButtonDown();	
	}
}