using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float _hitpoints = 3f;			//How many hits can the enemy take before exploding
	public float _maxSpeed;			//Enemy max velocity
	public  GameObject _shield;				//Shield pops up when enemy collides with something (bullets still do damage)
	public ParticleSystem _explosion;		//Explosion when enemy dies
	public GameObject _model ;				//Model of enemy
	public Bullet _bullet;					//Bullet that enemy shoots
	public float _shootEverySecond = 1f;	//How often enemy shoots
	public float _shootChance  = .5f;		//Chance that enemy fires off a bullet when shooting
	public int _points = 1000;				//Points player gets when enemy is defeated
	public float _rotateBackForce = 300f;	//How much force to add
	public bool _dropPickupOnHit = true;//Drops loot when hit by a bullet
	public float _dropChance = 1f;			//0=never 1=always

	private float _shieldCounter;	//Counter to deactivate shield
	private  Vector3 _force;			//Force added to enemy to move it
	private bool _dead ;			

	public int scorepoint;

	public ParticleSystem _particleSystem;	//Particle system emitted from enemy
									
	void Start ()
	{
		GameController.instance._enemyCounter++;
		InvokeRepeating("RandomDirection", 0 , 2);	//Change force direction every 2 seconds
		if(_shield){								//Deactivate shield
			_shield.SetActive(false);
		}
		if (_maxSpeed == 1)
			_shootEverySecond = 2f;
		else if (_maxSpeed == 2)
			_shootEverySecond = 2.5f;
		else
			_shootEverySecond = 4.5f;
		InvokeRepeating("Shoot", _shootEverySecond , _shootEverySecond);	//Begins shooting routine
		scorepoint = (int)_hitpoints;
	}


	void Shoot ()
	{
		if(Random.value<.9f){																//Only shoots if value is smaller than _shootChance
			Quaternion rot=new Quaternion();				
			rot.SetLookRotation(Player.instance.transform.position - transform.position); 	//Rotate towards player
			Bullet b = (Bullet)Instantiate(_bullet, transform.position, rot);					//Create a bullet on the stage
		}
	}


	void DestroyEnemy ()
	{
		if(!_dead){													//Make sure that this void is only run once
			if (GameController.instance.canUFO1 && Time.time - GameController.instance.zeroTime1 > 12f - GameController.instance.bkno / 2)
			{
				GameController.instance.canUFO1 = false;
				GameController.instance.zeroTime1 = Time.time;
			}
			if (GameController.instance.canUFO2 && Time.time - GameController.instance.zeroTime2 > 12f - GameController.instance.bkno / 2)
			{
				GameController.instance.canUFO2 = false;
				GameController.instance.zeroTime2 = Time.time;
			}
			GameController.instance._enemyCounter--;				//Decrease enemycounter in GameController
			GameController.instance.ufonumber--;					//Decrease UFO Count in GameController
			_explosion.transform.position = transform.position;		//Positiones the explosion on enemy position
			_explosion.Play();										//Play explosion particle system
			_dead = true;											//Enemy no longer alive but still active
			_model.SetActive(false);									
			this.GetComponent<Collider>().enabled = false;
			_particleSystem.Stop();
			_shield.SetActive(false);
			CancelInvoke();											//Stops shooting
			GetComponent<Rigidbody>().velocity*=.1f;									//Slows down speed
			if(scorepoint == 5)				//Add points to score
				GameController.instance.AddScoreMultiplier(1f);
			else if(scorepoint == 8)
				GameController.instance.AddScoreMultiplier(1.5f);
			else
				GameController.instance.AddScoreMultiplier(2f);
			GameController.instance.InvokeEnemies ();				//Starts creating new enemies
			StartCoroutine(WaitDestroy());
		
		}
	}

	IEnumerator WaitDestroy(){
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);									//Removes enemy completely
	}


	void FixedUpdate () {
		
		GetComponent<Rigidbody>().AddForce(_force);								//Add movement to enemy	
		//Gradually rotates enemy to zero
		if(transform.rotation.y < 0 ){
			transform.GetComponent<Rigidbody>().AddTorque(0,_rotateBackForce*Time.deltaTime *-transform.rotation.y,0);
		}else if(transform.rotation.y > 0){
			transform.GetComponent<Rigidbody>().AddTorque(0,_rotateBackForce*Time.deltaTime *-transform.rotation.y,0);
		}
		
		//Makes sure the enemy velocity dont go faster than max speed
		if(GetComponent<Rigidbody>().velocity.sqrMagnitude > _maxSpeed){ 
	        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * _maxSpeed;
	    } 
	}


	void OnCollisionEnter (Collision col) {
		if(_hitpoints<1){
			DestroyEnemy();			//Destroy enemy if it has no hitpoints left
		}else{
			if(_dropPickupOnHit && col.gameObject.tag == "Bullet" && _dropChance > Random.value)	//Drops item when hit by bullet
				GameController.instance.Drop(transform.position);
		}
	}


	void DeActivateShield () {
		if(_shield){
			_shield.SetActive(false);	//Deactivates shield
		}
	}


	void ActivateShield () {	//Activate shield (Shield is just visual and has no real effect)
		if(_shield){
			_shieldCounter = 0;		//Resets the counter that deactivates shield
			_shield.SetActive(true);	
		}
	}


	void LateUpdate () {
		if(_shieldCounter > 1){					//Check if it is time to deactivate the shield
			DeActivateShield();
		}else{
			_shieldCounter += Time.deltaTime;	//Increase shield counter
		}
		if(transform.position.y < -0.01){													//Checks to see if enemy is on the game plane
			float y0 = Mathf.Lerp(transform.position.y, 0, Time.deltaTime*GameController.instance._asteroidAlignToPlaneSpeed); //Gradually positiones the asteroid closer to the game plane
			transform.position=new Vector3(transform.position.x,y0,transform.position.z);
		}else{
			GameController.instance.CheckBounds(this.gameObject, this._particleSystem); 	// Check if the enemy is in the game area, moves it to the closest edge if it is outside
			transform.position=new Vector3(transform.position.x,0f,transform.position.z);// Keeps the enemy on the game plane
		}
	}


	void RandomDirection () {				//Change the direction of the enemy
		_force = new Vector3(Random.Range(-1000, 1000), 0 ,Random.Range(-1000, 1000)); 
	}
}