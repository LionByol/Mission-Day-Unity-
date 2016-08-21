using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
    public int stoneType = 1;
	public float _hitpoints = 3f;			//How many hits a asteroid can take from a default bullet
	public int _points = 100;				//How many point player gets from destroying this asteroid
	public GameObject _breakInto;			//Contains the prefab that this asteroid spawnes when it is destroyed (can be blank)
	public int _debrisMultiplier = 1;	    //Change how much debris should be emitted from the _debrisPS particle system
	public float _dropChance;				//Chance asteroid will drop an item (multiplied by the GameController._dropMultiplier)
	public float _maxAsteroidSize = 1.25f;	//Asteroids are scaled at start for publiciety
	public float _minAsteroidSize = .75f;
	public ParticleSystem _debrisPS;        //The particle system that emits debris (this must be present on the stage as "Debris PS"

	void Start () {

		GameController.instance._asteroidCounter++;				//Increase counter that checks how many asteroids are on the stage
		if(!_debrisPS){
			_debrisPS = GameObject.Find("Debris PS").GetComponent<ParticleSystem>();
		}

        _hitpoints *= stoneType;
	}

	void FixedUpdate() {
		//Max / Min Astreoid velocity (controlled by GameController)
		//Fixes issues where Asteroids builds up too much speed or stops completely, also good for altering difficulty
	    if(GetComponent<Rigidbody>().velocity.sqrMagnitude > GameController.instance._asteroidMaxVelocity){ 
	        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * GameController.instance._asteroidMaxVelocity;
	    } else if(GetComponent<Rigidbody>().velocity.sqrMagnitude < GameController.instance._asteroidMinVelocity){
	        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * GameController.instance._asteroidMinVelocity;
	    }
	    if(transform.position.y > -0.5)
	    GameController.instance.CheckBounds(this.gameObject, null); 	// Check if the asteroid is in the game area, moves it to the closest edge if it is outside
	}

	void Update () {
		if(_hitpoints < 0){											//When asteroid no longer has any hitpoints left
			int emit;
			if(this._breakInto){			
				GameController.instance.SpawnAsteroid(_breakInto, gameObject);

				emit = 10*_debrisMultiplier;						//How many debris pieces to emit when asteroid breaks into pieces 
			}else{
				emit = 25*_debrisMultiplier;						//How many debris pieces to emit when asteroid is destroyed	
			}
			_debrisPS.transform.position = transform.position;		//Set Particle System position on asteroid position
			_debrisPS.startColor = GetComponent<Renderer>().sharedMaterial.color;	//Change color of debris to match asteroid
			_debrisPS.Emit(emit);									//Start emission	 

			GameController.instance._asteroidCounter--;				//Decrease counter that checks how many asteroids are on the stage
			if(_dropChance * GameController.instance._dropMultiplier > Random.value){
				GameController.instance.Drop(transform.position);
			}
			GameController.instance.InvokeAsteroids();				//Start routine to check if game should spawn more asteroids
			Destroy(gameObject);
		}else{	
			float yPos=0f;
			if(transform.position.y < -0.01){						//Checks to see if asteroid is on the game plane
				yPos = Mathf.Lerp(transform.position.y, 0, Time.deltaTime*GameController.instance._asteroidAlignToPlaneSpeed); //Gradually positiones the asteroid closer to the game plane
			}
			transform.position=new Vector3(transform.position.x,yPos,transform.position.z);
		}
	}
}
