using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	//Bomb pickup properties
	public GameObject _bomb  ;			//Model of Bomb
	public AudioClip _bombSound ;		//Sound when Bomb picked up
	//Life pickup properties
	public GameObject _life ;			//Model of Life
	public AudioClip _lifeSound ;		//Sound when Life picked up
	//Point pickup properties
	public GameObject _point ;			//Model of Point
	public AudioClip _pointSound ;		//Sound when Point picked up
	//Special pickup properties
	public GameObject _specialBullet;	//Model of Special Bullet

	public float _lifetime  = 10.0f;		//How long is the pickup item alowed to be on the stage
	public string _type ;				//Name of the Pickup object (bomb, life, point etc)

	private  bool _pickedUp;	//Has this object been picked up (start the movement towards player)
	private Vector3 _moveFrom ;	//Postion of the Pickup once it gets picked up
	private  int _moveCounter;	//Counter for movement over time(Lerp)
	private GameObject _model;	//Gameobject based on what type of item this is


	void Start () {
		float _specialDropRate  = 0.45f;				//chance that special item drop
		float _bombDropRate 	= 0.03f;				//chance that bomb item drop
		float _lifeDropRate  = 0.0075f;
		if(_type == ""){
			float r  = Random.Range(0f, 1.0f);
			if (r < _lifeDropRate && (GameData.bonuslife == 0 || GameData.bonuslife == 1))
				_type = "life";								//Check if life should drop
			else if (r < _bombDropRate && Player.instance._currentBullet < Player.instance._bullets.Length - 1)
				_type = "bomb";		//If no life is dropped, check for bomb
			else if (r < _specialDropRate) {
				_type = "specialbullet";
//				GameData.missileCount--;
			}
			else _type = "point";							//If no other drops, drop point
		}

		transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
		//Activate model based on _type
		if(_type == "bomb"){
			_bomb.SetActive (true);
			_life.SetActive (false);
			_specialBullet.SetActive (false);
			_model = _bomb;
		}else if(_type == "point"){
			_point.GetComponent<Renderer>().enabled = true;
			_life.SetActive (false);
			_bomb.SetActive (false);
			_specialBullet.SetActive (false);
			_model = _point;
		}else if(_type == "life"){
			_life.SetActive (true);
			_bomb.SetActive (false);
			_specialBullet.SetActive (false);
			_model = _life;
		}else if(_type == "specialbullet"){
			_specialBullet.SetActive (true);
			_life.SetActive (false);
			_bomb.SetActive (false);
			_model = _specialBullet;
		}
		StartCoroutine(AutoDestroy());													//Start the countdown to remove object automaticly
		GameController.instance.CheckBoundsInverted(gameObject, null, 1);	//Checks if gameObject is outside the game area
	}

	//Call to activate the movement towards player
	public void Pickup0 () {	
		_pickedUp = true;
		_moveFrom = this.transform.position;
	}

	//Blinking before destroyed
	IEnumerator AutoDestroy () {	
		if (_type == "point") {
			yield return new WaitForSeconds (_lifetime - 3);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.5f);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.2f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.4f);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.3f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.3f);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.2f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.2f);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
		} else {
			yield return new WaitForSeconds (_lifetime - 3);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.5f);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.2f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.4f);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.3f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.3f);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.2f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.2f);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.1f);
			_model.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.1f);
		}
		Destroy(this.gameObject);
	}



	void FixedUpdate () {
		if(!Player.instance._inControl){		//Destroy pickup if player is dead
			Destroy(this.gameObject);
		}else if(!_pickedUp){		
			if(Vector3.Distance(Player.instance.transform.position, this.transform.position) < 1.4f ){	//Pickup if close to player
				this.Pickup0();	
			}	
		}else{
			_moveCounter++;																										//Increase counter for movement over time(Lerp)
			_model.GetComponent<Renderer>().enabled = true;			
			StopCoroutine("AutoDestroy");																		
			transform.position = Vector3.Lerp(_moveFrom, Player.instance.transform.position, Time.deltaTime*_moveCounter*2);	//Move towards player
			if(Vector3.Distance(Player.instance.transform.position, this.transform.position) < .2f){							//Check distance to player, if close enough it will trigger event based on _type	
				if (_type == "point") {																							//Adds to score multiplier
					if (_pointSound)
						SoundController.instance.Play (_pointSound, 2, Random.Range (1.5f, 1.25f));						//Play sound
					GameController.instance.AddScoreMultiplier (0.5f);
				} else if (_type == "bomb") {																						//Upgrade weapon
					if (_bombSound)
						SoundController.instance.Play (_bombSound, 2, 1.25f);											//Play sound
					Player.instance.UpgradeWeapon ();
				} else if (_type == "life") {
					GameData.bonuslife++;
					if (_lifeSound)
						SoundController.instance.Play (_lifeSound, 2, 1.25f);   
					Player.instance.IncreaseLife ();
				} 
				else if (_type == "specialbullet") {
					if (_bombSound)
						SoundController.instance.Play (_bombSound, 2, 1.25f);
					GameData.specMissiles += 1;
				}
				Destroy(this.gameObject);					//Remove gameObject
			}
		}
//		if (Vector3.Distance (Player.instance.transform.position, this.transform.position) < .65f && _type == "specialbullet") {
//			if (_bombSound)
//				SoundController.instance.Play (_bombSound, 2, 1.25f);
//			GameData.specMissiles += 1;
//			Destroy(this.gameObject);
//		}
	}
}