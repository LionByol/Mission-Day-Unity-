using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int maxHp; //1 to 7
	public Bullet _bullet;							//Contains bullet to be fired when shooting
    public int _shootingCount;
	public  bool _clickToPickup;					//Enable if user can touch/click pickup objects to pick them up
	public GameObject _shipModel;					//Model of the spaceship
	public GameObject _shield;						//Shield model
	public ParticleSystem rocket;					//Rocket particle system
	public ParticleSystem _explosion;				//Explosion particle system when player dies
	public GameObject _moveTo;						//Player always follows this around
	public GameObject _moveLook;					//Rotation of this controls direction of players movement, added to make the ship be able to rotate towards the mouse/touch but move forward based on this object
	[HideInInspector]
	public int _lives = 3;							//Players lifes
    [HideInInspector]
    public int curHp = 0;
	public int _maxLives = 1;						//Players maximum allowed lives (Remember to change the GUI prefab lives if this is increased)
	public GameObject _bulletPoint;					//GameObject that controls where the bullets spawn (look in player prefab)
	public Bullet[] _bullets;						//List of bullets
	public int _currentBullet;						//Index of current bullet in list
	public AudioClip _audioDeath ;					//Audio clip when player dies
	public RaycastHit hit ;							//Mouse/touch position
	public float _speed = 0.5f;						//Ship movement speed

	private bool _invunerable;			//Can the player be hurt
	[HideInInspector]
	public bool _inControl = true;		//User control of the ship
	private bool _shooting ;				//Ship shooting

	private Vector3 pos ;					//Raycast position
	private Vector3 beforePos ;					//Raycast position
	private bool isDown ;					//Raycast position
	static public Player instance ;				// Player is a singleton. Player.instance.DoSomeThing();
	private bool isMouseMove = false;
    
	float shootSpeed;

	void Awake(){
		instance=this;
	}

	void OnApplicationQuit() {				// Ensure that the instance is destroyed when the game is stopped in the editor.
	    instance = null;
	}

	void Start () {
		shootSpeed=1f-_shootingCount*0.05f;
		//Setting parent to null is very tough on mobile, if player is instantiated this has to be changed
		_moveTo.transform.parent = null;		//Put follow object in the stage root to avoid it flailing around with the players movement
		_explosion.transform.parent = null;
		_bulletPoint.SetActive(false);			//Disables the bullet spawn point model	
	    _invunerable = false;								//Player is no longer invunerable
		GetComponent<Collider>().enabled = true;							//Enable collider
		StartCoroutine(Init());

		GUIManager.instance.ShowLive (3-curHp);
	}

	IEnumerator Init(){
		yield return new WaitForSeconds(.1f);
		if (LevelManager.instance)
			LevelManager.instance.CheckPlayer();	//Update player stats (GUI)
		isDown=false;
	}

	//Adds bullets to the stage(note: add multiple weapon types? _type if(_type))
	void Shoot () {
		if(_inControl){							//Cannot shoot while disabled
			Bullet b= (Bullet)Instantiate(_bullet, _bulletPoint.transform.position, transform.rotation);	//Create a bullet on the stage
			_shooting = true;					//Player is shooting
		}
	}

	void StopShoot () {
		CancelInvoke();							//Stops shooting
	}

	void OnCollisionEnter (Collision col) {
		//If player hits a asteroid and is not invunerable/shielded
		if(!_invunerable){
			if (col.gameObject.tag == "Asteroid"){
				col.gameObject.GetComponent<Asteroid>()._hitpoints = -1;	//Destroy/break the asteroid
				DestroyShip();	//Disable ship for a while to reset position
			}else if (col.gameObject.tag == "Bullet"){
				DestroyShip();
			}
		}
	}

	public void IncreaseLife()
	{
		curHp--;
		GUIManager.instance.ShowLive (3-curHp);
	}

	//Disables the ship and player controll for a while and removes one life (note: add smooth reset position?)

	void DestroyShip () {
		curHp++;
		GameData.specMissiles = 0;
		GameData.bonuslife = 0;

		GUIManager.instance.ShowLive (3-curHp);
		ResetWeapon();	
        _explosion.transform.position = transform.position;		//Positiones the explosion ontop of player
        _explosion.Play();										//Play explosion particle system
		if (curHp >= maxHp) {
			_lives -= 1;
			curHp = 0;
			GameController.instance.SetScoreMultiplier (1);			//Reset the score multiplier when dead

			if (_audioDeath) {										//If there is audio on death
				SoundController.instance.StopAll ();					//Stop all other sounds (to make this sound more audiable)
				SoundController.instance.Play (_audioDeath, 2, 1);	//Play the death sound
			}
			_invunerable = true;									//Make ship take no damage for a limited time while resetting
			GetComponent<Collider> ().enabled = false;								//Disable ship collider
			_inControl = false;										//Player can no longer be controlled by user
            
			LevelManager.instance.CheckPlayer ();					//Update player stats (GUI)
			rocket.enableEmission = false;							//Disable the rocket particle system
            
			_shipModel.SetActive (false);
			transform.position = Vector3.zero;						//Set position to absolute zero
            
			_moveTo.transform.position = transform.position;		//Reset the moveTo gameObject
			GameController.instance.GameOver (true);			//GameOver if lives are less than zero (false indicates that the level was not completed)
		} else {
			StartCoroutine (invunerable ());
		}
	}

	IEnumerator Invunerable (float sec )
	{
		_shield.SetActive(true);									//Show shield model
		yield return new WaitForSeconds(sec);								//Delay
			_shield.SetActive(false);								//Disable shield
			_invunerable = false;								//Player is no longer invunerable
			GetComponent<Collider>().enabled = true;							//Enable collider
	}

	//Make the ship invunerable for a duration (Note: in progress)
	IEnumerator invunerable(){
		_invunerable = true;
		_shield.SetActive(true);	
		yield return new WaitForSeconds(2);
		_invunerable = false;
		_shield.SetActive(false);	
	}

	//Upgrade weapon by picking the next Bullet in the _bullets list
	public void UpgradeWeapon () {
		if(_currentBullet < _bullets.Length-1){
			StopShoot();													//Stops shooting so that the Shoot void get updated
			_currentBullet++;
			this._bullet = this._bullets[this._currentBullet];
			if(_shooting)											//Shoot if user touches for a while/holds
                Invoke("Shoot", 0);
            StartCoroutine(DownWeapon());
		}
	}

    IEnumerator DownWeapon()
    {
        yield return new WaitForSeconds(8);
        if (_currentBullet > 1)
        {
            StopShoot();													//Stops shooting so that the Shoot void get updated
            _currentBullet--;
            this._bullet = this._bullets[this._currentBullet];
            if (_shooting)											//Shoot if user touches for a while/holds
                Invoke("Shoot", 0);
        }        
    }

	//Set weapon to the firs Bullet in the _bullets list (Happens when player dies)
	void ResetWeapon () {
		_currentBullet=1;
		this._bullet = this._bullets[1];
	}


	void LateUpdate () {
		if (Application.loadedLevelName!="Battle")
			return;
		if(hit.point != transform.position){
			Quaternion rotation = Quaternion.LookRotation(hit.point - transform.position);		//Calculate rotation
			transform.rotation = rotation;
		}
	    _moveLook.transform.LookAt(_moveTo.transform.position, transform.up);						//Player movement direction 
		float  d  = Vector3.Distance(transform.position, _moveTo.transform.position);			//Distance between Player and where it is moving to
	   	GetComponent<Rigidbody>().velocity = _moveLook.transform.forward*_speed*d;									//Move the Player based on _moveLook rotation
	    if(d < .5){																					//Disable _moveTo model if Player is close (GUI)
	    	if(_moveTo.activeInHierarchy)_moveTo.SetActive(false);														
	    }else if(_inControl){																		//Enable _moveTo model if not
	    	if(!_moveTo.activeInHierarchy) _moveTo.SetActive(true);
	   		_moveTo.transform.Rotate(new Vector3(0,100*Time.deltaTime,0));								//Rotate _moveTo model (visual)
	   	}
		if(Input.GetMouseButtonDown(0) ){	//Shoot if user touches for a while/holds
			_shooting = false;							//Player is not shooting
			isDown=true;
			beforePos=hit.point;
        }
		else if (Input.GetMouseButtonUp(0))
        {										//Move if user clicks/single touches game area
			if(!isMouseMove){
				ShootCount();
                TutorialManager.instance.CheckTap();
			}
			if((_shooting && (!hit.collider.GetComponent<Pickup>()||!_clickToPickup)) && Time.timeScale == 1){				
				_moveTo.transform.position = hit.point;												//Move the _moveTo to the touch position if no pickup is registered
				_shooting = true;
			}
			else if(_clickToPickup && hit.collider.GetComponent<Pickup>()){
				hit.collider.GetComponent<Pickup>().Pickup0();											//Pickup item		
			}	 
			isMouseMove = false;
			isDown=false;
		}
		else if (isDown){		//Move Event
		 	if (Vector3.Distance(beforePos,hit.point)>0.2f){
				_moveTo.transform.position = hit.point;	
				isMouseMove = true;
                TutorialManager.instance.CheckSwip();
			}
			beforePos=hit.point;
		}
	}

    void ShootCount()
    {
        for (int i = 0; i < _shootingCount; i++)
        {
            Invoke("Shoot", i*0.4f);
        }
    }

	void Update () {
		if (Application.loadedLevelName!="Battle")
			return;
	   	#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_STANDALONE || UNITY_WEBPLAYER
	       pos = Input.mousePosition ;			//Get position based on mouse
	    #endif
	    #if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
	    	if (Input.touchCount > 0)
	    	pos = Input.GetTouch(0).position ;		//Get position based on touch  
	    #endif
	    if(Physics.Raycast(Camera.main.ScreenPointToRay(pos),out hit)){  
			hit.point=new Vector3(hit.point.x,0f,hit.point.z);	   		 
	    }
	//    Debug.DrawLine(transform.position, hit.point);												//Draw a line to visualize where the user is touching on the game area (editor visual)	

		//monitor UFO
		float dist = Vector3.Distance (Vector3.zero, gameObject.GetComponent<Rigidbody> ().velocity);
		print (dist);
		if (GameController.instance.ufonumber > 0) {
			if (!activeSpecial && gameObject.activeSelf &&  dist>=5f) {
				StartCoroutine (FireSpecialMissile ());
				activeSpecial = true;
			}
		} else
			activeSpecial = false;
	}

	IEnumerator FireSpecialMissile()
	{
		yield return new WaitForSeconds (1f);
		while (true)
		{
			if(GameData.specMissiles > 0)
			{
				SpecialShoot ();
				GameData.specMissiles--;
			}
			else if(GameData.specMissiles == 0)
			{
				activeSpecial = false;
				break;
			}
			yield return new WaitForSeconds (1f);
		}
	}

	void SpecialShoot ()
	{
		Quaternion rot=new Quaternion();				

		//enemies
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		//get distances of all ufos
		float[] dises = new float[enemies.Length];
		for (int i = 0; i < dises.Length; i++)
			dises [i] = Vector3.Distance (transform.position, enemies [i].transform.position);
		//get nearest ufo
		int min = 0;
		for (int i = 0; i < dises.Length; i++)
			if (dises [min] > dises [i])
				min = i;
		Vector3 selfPos = transform.FindChild ("sPos").position;

		//get direction and position
		rot.SetLookRotation(enemies[min].transform.position - selfPos); 	//Rotate towards player
		if(enemies[min].transform.position.y <=0.02f)
		{
			Bullet b = (Bullet)Instantiate(specialMissile, selfPos, rot);	//Create a bullet on the stage
		}
	}

	private bool activeSpecial;
	public Bullet specialMissile;
}