using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public ParticleSystem _rocket;			//Bullet is built up entirely by particles
	public ParticleSystem _explosion;		//Explosion particle system on collision
	public float _rotationOffset;			//Shoot bullet sideways
	public float _RandomRotationOffset;	    //Randomly shoot bullet sideways
	public float _spreadDelay = .25f;		//How long until the bullets loose their trajectory (A more visually pleasing way to limit the bullets range alowing them to live longer but less effective after delay)
	public float _spreadAmount = .5f;		//How much the bullets spread after spread delay
	public float _bulletLifetime = 3f;		//How long the bullet is allowed to live without hitting anything (seconds)
	public float _bulletPower  = 1f;		//Hit points removed from hit object
	public float _fireRate  = 0.1f;			//Bullets rate of fire, low = fast
	public AudioClip _audioBirth;			//Audio clip to play when object is created
	public Vector2 _pitchRandom = new Vector2(.5f,1.5f);
	public AudioClip _audioDeath;			//Audio clip to play when object is destroyed
	public bool _wrapGameBorders ;		    //Bullets that leaves game area are moved to the opposite side
	public float  _bulletSpeed = 5f;		//Speed of bullets
	private bool _spread ;
	private Quaternion rotTarget;	        //Rotation used for direction


	void Start () {
		if(_audioBirth)	
			SoundController.instance.Play(_audioBirth, Random.Range(.5f,1f), Random.Range(_pitchRandom.x,_pitchRandom.y));	//Audio on birth
		GetComponent<Rigidbody>().transform.Rotate(0f,_rotationOffset+Random.Range(-_RandomRotationOffset, _RandomRotationOffset),0f);

		rotTarget = GetComponent<Rigidbody>().rotation;				//set rotation for direction
		
		Spread();									//Randomly rotates the game object to change direction (delayed)
		DestroyBullet ();							//Removes the bullet from the stage (delayed)
		
		StartCoroutine(StartYield());		        //disable the particle emission before destroying (visual)

	}

	IEnumerator StartYield(){
		yield return new WaitForSeconds(_bulletLifetime);
		_rocket.enableEmission = false;
		GetComponent<Collider>().enabled = false;
	}
	//Removes the bullet from the stage
	void DestroyBullet () {
		StartCoroutine(DestroyYield());//Wait for _bulletLifetime before destroying the bullet (delayed so that the rocket particle system has time to stop emission)
	}

	IEnumerator DestroyYield(){
		yield return new WaitForSeconds(_bulletLifetime+.75f);
		Destroy(this.gameObject);
	}

	//Randomly rotates the game object to change direction (delayed)
	void Spread () {
		StartCoroutine(SpreadYield());//Wait for _spreadDelay before making bullet steer off course
	}

	IEnumerator SpreadYield(){
		yield return new WaitForSeconds(_spreadDelay);
		randomRot ();							
		_spread = true;							//Indicate that the bullet now should move towards the random rotation
	}

	void OnCollisionEnter (Collision col) {
		GameObject e = (GameObject)Instantiate(_explosion.gameObject, transform.position, GetComponent<Rigidbody>().rotation);
		Destroy(e, _explosion.duration);																				//Destroy the bullet (Delayed)
		_rocket.enableEmission = false;																					//Disable the rocket particle system
		transform.GetComponent<Collider>().enabled = false;																				//Disable collider to prevent more collsions since rocket needs to stop before destroying
		if(_audioDeath)
			SoundController.instance.Play(_audioDeath, Random.Range(.5f,1f), Random.Range(.5f,1.5f));		//Audio on death	
		if(col.transform.tag == "Asteroid")																				//Hit an asteroid
			col.transform.GetComponent<Asteroid>()._hitpoints-=_bulletPower;												//Reduce hitpoints of hit object based on bullet power
		else if(col.transform.tag == "Enemy")																			//Hit an Enemy
			col.transform.GetComponent<Enemy>()._hitpoints-=_bulletPower;													//Reduce hitpoints of hit object based on bullet power
		Destroy(gameObject);
	}

	void FixedUpdate () {
		GetComponent<Rigidbody>().velocity = transform.forward*_bulletSpeed;															//Movement forward
		if(_spread)GetComponent<Rigidbody>().rotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, rotTarget, Time.deltaTime*_spreadAmount);	//Rotates to a random rotation, rotation speed is based on _spreadAmount
		if(_wrapGameBorders) GameController.instance.CheckBounds(this.gameObject, _rocket);								//Checks if the bullet is outside the game area
	}

	//Randomly rotates the gameobject to change direction (smooth)
	void randomRot () {
		 rotTarget = Random.rotation;
	}
}