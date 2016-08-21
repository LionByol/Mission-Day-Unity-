using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//Difficulty modifiers are best used in survival mode
	[HideInInspector]  public int _difficultyIncreaseAsteroidCount = 0 ;	//Ever x amount of asteroids spawned will increase difficulty
	[HideInInspector]  public float _maxDifficulty  = 1;					//Difficulty will stop increasing after reaching this value

	[HideInInspector]  public float _asteroidMinVelocity  = 1;				//Keeps Asterois from stopping to prevent dull gameplay
	[HideInInspector]  public float _asteroidMaxVelocity  = 2;				//Keeps Asterois from gaining too much velocity
	public Asteroid[] _asteroids;											//List of asteroids to spawn at random
	public Material[] _asteroidMaterials ;									//Materials will be changed randomly on asteroids if this list contains any (adds some visual varaity to the asteroids)
	[HideInInspector]  public float _asteroidSpawnDelay  = 0.3f;			//Delay between each new asteroid
	[HideInInspector]  public int _maxAsteroids  = 2;						//Max asteroids and fragment on the screen (will not spawn new asteroids when there are this many in the game)

	[HideInInspector]  public float _asteroidAlignToPlaneSpeed  = 2;		//How fast asteroids moves to the game area (bigger number makes it harder to avoid new asteroids, can be used for increasing difficulty)
	[HideInInspector]  public bool _spawnAsteroidsWhenEnemy = true;			//Turn this on if asteroids should spawn even if a enemy is on stage
	[HideInInspector]  public float _spawnAsteroidsWhenEnemyDifficultyOveride  = 2.5f;


	public Enemy[] _enemy ;													//List of enemies like UFO that spawns less frequently than asteroids
	[HideInInspector]  public int _maxEnemy  = 1;							//Max enemies in the game at once

	[HideInInspector]  public float _spawnEnemyEverySecond  = 0.1f;
	[HideInInspector]  public float _spawnEnemyChance  = .2f;				//Chance that an enemy will be spawned 1=always 0=never
	//Powerups, lives and score multipliers
	public Pickup _pickup;													//Pickup objects that drops
	[HideInInspector]  public float _dropMultiplier  = 1000;				//chance that items drop 
	[HideInInspector]  public float _specialDropRate  = 0.05f;				//chance that special item drop
	[HideInInspector]  public float _bombDropRate 	= 0.01f;				//chance that bomb item drop
	[HideInInspector]  public float _lifeDropRate  = 0.001f;				//chance that life item drop

	[HideInInspector]  public int _score = 0;
	[HideInInspector]  public int _maxScoreMultiplier = 100;
	//Anchors (GUI)
	//Attach gameObject containing 3D GUI elements that is to be aligned to the screen
	//public GameObject _guiBottomLeft ;

	[HideInInspector]  public int _scoreMultiplier  = 1;					//Score is multiplied by this value
	[HideInInspector]  public int score = 0;
	[HideInInspector]  public int _toPoints ;
	[HideInInspector]  public int _asteroidCounter ;
	[HideInInspector]  public int _enemyCounter ;

	[HideInInspector]  public int _spawnAsteroidAmount  = 5;				//-1 will spawn asteroids infinetly (survival mode)
	[HideInInspector]  public int _spawnEnemyAmount  = 5;

	[HideInInspector]  private float _gameWidth ;							//Width of the game area
	[HideInInspector]  private  float _gameHeight;							//Height of the game area
	//Calculated half of the screen; limits the calculation to once the game starts
	[HideInInspector]  private  float _gameWidthHalf;	
	[HideInInspector]  private  float _gameHeightHalf;
	//These values contain information about the edges of the screen, calculated once the game start
	private  RaycastHit _bottomLeft ;
	private  RaycastHit _bottomRight ;
	private  RaycastHit _topRight ;
	private  RaycastHit _topLeft;
	private  int _spawnEnemyCounter ;		
	private  int _totalAsteroidsCounter;	

	private int _greyStoneAmount;
	private int _blueStoneAmount;
	private int _redStoneAmount;

	private int _greyStoneCounter = 0;
	private int _blueStoneCounter = 0;
	private int _redStoneCounter = 0;

    private int _blueUFOAmount;
    private int _redUFOAmount;
    private int _greenUFOAmount;
    
    private int _blueUFOCounter = 0;
    private int _redUFOCounter = 0;
    private int _greenUFOCounter = 0;
    
	public static  GameController instance;			// GameController is a singleton.	 GameController.instance.DoSomeThing();

	void Awake(){
		instance=this;
	}

	void OnApplicationQuit() {						// Ensure that the instance is destroyed when the game is stopped in the editor.
	    instance = null;
	}

	void Start ()
	{
		score = GameData.startScore;
		GUIManager.instance.ShowScore(score);

//		GameData.missileCount = 0;
		ufonumber = 0;
		GameData.bonuslife = 0;
		GameData.specMissiles = 0;
        _maxEnemy = 1 + (int)(GameData.selectedLevel*13/12);
		_blueUFOAmount = (GameData.selectedLevel - 1) / 2;
        _redUFOAmount = (GameData.selectedLevel - 1) / 3;
        _greenUFOAmount = (GameData.selectedLevel - 1) / 4;
        _spawnEnemyAmount = _blueUFOAmount + _redUFOAmount + _greenUFOAmount;

        GameData.killedUfo = 0;

        _maxAsteroids = 2 + (int)(GameData.selectedLevel / 10);
        _greyStoneAmount = 10 * (GameData.selectedLevel + 1);   //levelData.greyStone;
        _blueStoneAmount = 5 * (GameData.selectedLevel - 1);    //levelData.blueStone;
        _redStoneAmount = 2 * (GameData.selectedLevel - 1);     //levelData.redStone;
        _spawnAsteroidAmount= _greyStoneAmount + _blueStoneAmount + _redStoneAmount;

		CalcBounds();										//Calculates the game area based on camera edges by casting rays from all corners of the main camera
		InvokeRepeating("CountPoints", 1f, .01f);			//Repeating void that counts the score
		InvokeAsteroids();									//Spawns asteroids until max spawn amount has been reached
		InvokeEnemies ();									//Spawns enemies until max spawn amount has been reached

		GUIManager.instance.ShowStone(0, _spawnAsteroidAmount);
		GUIManager.instance.ShowEnemy(0, _spawnEnemyAmount);
	}

	private int tmp = -1;
	private int madeUFOs = 0;
	public int bkno;
	private bool checked0 = false;
//	private bool checked1 = false;
	void Update()
	{
		if (!checked0 || score != tmp && score % 1000 == 0)		//up to by 1000
		{
//			if (score < 15000) {
//				checked0 = true;
//				bkno = Mathf.FloorToInt (score / 1000);
//				GameData.missileCount = GameData.mvs [bkno];
//				madeUFOs = 0;
//			}
//			if (!checked1 && score >= 15000 && score < 24000) {
//				checked1 = true;
//				bkno = 15;
//				GameData.missileCount = GameData.mvs [bkno];
//				madeUFOs = 0;
//			}
//			if (score >= 24000) {
				checked0 = true;
				bkno = Mathf.FloorToInt (score / 1000);
//				GameData.missileCount = GameData.mvs [bkno];
				madeUFOs = 0;
//			}
		}
		tmp = score;
//		if (zeroTime == 0 && ufonumber == 0) {
//			zeroTime = Time.time;
//		}
//		GUIManager.instance.ufonumber.text = "Total on block: "+madeUFOs+"\nCurrent on screen: "+ufonumber;
	}

	//Called by asteroids to create new asteroids, prefabs instantiating prefab of same class can not be done
	public void SpawnAsteroid (GameObject go , GameObject spawner) {	
		GameObject asteroid  = (GameObject)Instantiate(go, spawner.transform.position, spawner.transform.rotation);
        Asteroid[] children;
		children = asteroid.GetComponentsInChildren<Asteroid>(true);			//Makes a list of all the asteroid children
		for (int i=0; i < children.Length; i++) {
			children[i].GetComponent<Renderer>().sharedMaterial =spawner.GetComponent<Renderer>().sharedMaterial;
            children[i].GetComponent<Asteroid>().stoneType = spawner.GetComponent<Asteroid>().stoneType;
			children[i].GetComponent<Rigidbody>().velocity = spawner.GetComponent<Rigidbody>().velocity;		//Copy the velocity of main asteroid to children when it breaks	
			children[i].transform.localScale = spawner.transform.localScale;	//Resizes asteroids	//Might generate Android lag (Noticed some tiny lagspikes)
		}
	}

	public int ufonumber;			//ufo numbers
	public float zeroTime1 = 0;
	public float zeroTime2 = 0;
	public bool canUFO1 = false;
	public bool canUFO2 = false;
	void SpawnEnemy () 
	{
		float dT1 = Time.time - zeroTime1;
		float dT2 = Time.time - zeroTime2;
		float stT = 12f - GameController.instance.bkno/2;
		if (stT < 0f)
			stT = 1f;
		
		if (!canUFO1 && dT1 > stT)
		{
			canUFO1 = true;
		}else if (!canUFO2 && dT2 > stT)
		{
			canUFO2 = true;
		}
		if ((canUFO1 || canUFO2) && Random.Range(0f, 1f)<0.5f && ufonumber<2)
        {	//Check if all enemies in level has been spawned 
			//Debug.Log("running"  + _enemyCounter +" enemies "+_maxEnemy * Mathf.Floor(this._difficultyMultiplier));
//			int max;
//			if (GameData.vState == 1)
//				max = GameData.ufos1 [bkno];
//			else if (GameData.vState == 2)
//				max = GameData.ufos2 [bkno];
//			else
//				max = GameData.ufos3 [bkno];
//			if(madeUFOs < max/* || (ufonumber==0 && zeroTime!=0 && Time.time-zeroTime>3)*/){
//			if()
//			{
				int ufoType;
				if (bkno >= 5)
					ufoType = Random.Range (0, _enemy.Length);
				else if (bkno >= 3)
					ufoType = Random.Range (0, _enemy.Length - 1);
				else
					ufoType = 0;
				madeUFOs++;
				if (ufoType == 2 && bkno>=5)		//green UFO
                {
                    if (_greenUFOCounter == _greenUFOAmount)
                    {
                        if (_redUFOCounter == _redUFOAmount)
                        {
                            ufoType = 0;
                            _blueUFOCounter++;
                        }
                        else
                        {
                            ufoType = 1;
                            _redUFOCounter++;
                        }
                    }
                    else
                    {
                        _greenUFOCounter++;
                    }
                    
                }
				else if (ufoType == 1 && bkno>=3)		//red UFO
                {
                    if (_redUFOCounter == _redUFOAmount)
                    {
                        if (_greenUFOCounter == _greenUFOAmount)
                        {
                            ufoType = 0;
                            _blueUFOCounter++;
                        }
                        else
                        {
                            ufoType = 2;
                            _greenUFOCounter++;
                        }
                    }
                    else
                    {
                        _redUFOCounter++;
                    }
                }
				else
                {								//blue ufo
                    if (_blueUFOCounter == _blueUFOAmount)
                    {
                        if (_redUFOCounter == _redUFOAmount)
                        {
                            ufoType = 2;
                            _greenUFOCounter++;
                        }
                        else
                        {
                            ufoType = 1;
                            _redUFOCounter++;
                        }
                    }
                    else
                    {
                        _greenUFOCounter++;
                    }
                }
				GameObject enemy  = (GameObject)Instantiate(_enemy[ufoType].gameObject, new Vector3(Random.Range(-_gameWidthHalf, _gameWidthHalf)*.5f, Random.Range(-10, -20), Random.Range(-_gameHeightHalf, _gameHeightHalf)*.5f), Quaternion.identity);	
				GameData.killedUfo++;							//Counts how many enemies have spawned
                GUIManager.instance.ShowEnemy(GameData.killedUfo, _spawnEnemyAmount);
				ufonumber++;
//			}
		}
	}

	void SpawnAsteroids ()
	{									
		if(( _totalAsteroidsCounter < _spawnAsteroidAmount) && (_enemyCounter <=0 || _spawnAsteroidsWhenEnemy)){	//Check if all asteroids in level has been spawned 
			if(_asteroidCounter < _maxAsteroids-1)	{
				GameObject asteroid  = (GameObject)Instantiate(_asteroids[Random.Range(0, _asteroids.Length)].gameObject, new Vector3(Random.Range(-_gameWidthHalf, _gameWidthHalf), Random.Range(-10, -20), Random.Range(-_gameHeightHalf, _gameHeightHalf)), Quaternion.identity);
				Asteroid a = asteroid.GetComponent<Asteroid>();
				asteroid.transform.localScale *= Random.Range(a._maxAsteroidSize, a._minAsteroidSize);	//Resizes asteroids based on min/max variables	///Might generate Android lag (Noticed some tiny lagspikes)
				asteroid.GetComponent<Rigidbody>().AddForce(Random.Range(-5,5), 0 , Random.Range(-5,5));
				if(_asteroidMaterials.Length > 0){				//Changes material of asteroids if there are any in the array

					if(_totalAsteroidsCounter%9==8){
						if( _redStoneCounter<_redStoneAmount ){
							asteroid.gameObject.GetComponent<Renderer>().sharedMaterial = _asteroidMaterials[2];
                            asteroid.gameObject.GetComponent<Asteroid>().stoneType = 3;
							_redStoneCounter++;
						} else if( _blueStoneCounter<_blueStoneAmount ){
							if( _blueStoneCounter<_blueStoneAmount ){
								asteroid.gameObject.GetComponent<Renderer>().sharedMaterial = _asteroidMaterials[1];
                                asteroid.gameObject.GetComponent<Asteroid>().stoneType = 2;
								_blueStoneCounter++;
							}
						} else {
							asteroid.gameObject.GetComponent<Renderer>().sharedMaterial = _asteroidMaterials[0];
                            asteroid.gameObject.GetComponent<Asteroid>().stoneType = 1;
							_greyStoneCounter++;
						}
					} else if(_totalAsteroidsCounter%9==2||_totalAsteroidsCounter%9==5){
						if( _blueStoneCounter<_blueStoneAmount ){
							asteroid.gameObject.GetComponent<Renderer>().sharedMaterial = _asteroidMaterials[1];
                            asteroid.gameObject.GetComponent<Asteroid>().stoneType = 2;
							_blueStoneCounter++;
						} else {
							asteroid.gameObject.GetComponent<Renderer>().sharedMaterial = _asteroidMaterials[0];
                            asteroid.gameObject.GetComponent<Asteroid>().stoneType = 1;
							_greyStoneCounter++;
						}
					} else {
						asteroid.gameObject.GetComponent<Renderer>().sharedMaterial = _asteroidMaterials[0];
                        asteroid.gameObject.GetComponent<Asteroid>().stoneType = 1;
						_greyStoneCounter++;
					}
				
					//asteroid.gameObject.GetComponent<Renderer>().sharedMaterial = _asteroidMaterials[Random.Range(0,_asteroidMaterials.Length)];
					//Changes material of the asteroids children to match the new material
					Renderer[] children;
					children = asteroid.GetComponentsInChildren<Renderer>(true);	//Makes a list of all the renderers in the asteroid children
					for (int i=0; i < children.Length; i++) {
						children[i].GetComponent<Renderer>().sharedMaterial =asteroid.gameObject.GetComponent<Renderer>().sharedMaterial;
					}
				}
				_totalAsteroidsCounter++;						//Counts how many asteroids have spawned in total (Fragments not included)
				GUIManager.instance.ShowStone(_totalAsteroidsCounter, _spawnAsteroidAmount);
			}else{
				CancelInvoke("SpawnAsteroids");					//Stops spawning asteroids when max limit has been reached
			}
		}
	}

	public void InvokeEnemies () {	//Spawns enemies until max spawn amount has been reached
		CancelInvoke("SpawnEnemy");
		InvokeRepeating("SpawnEnemy", _spawnEnemyEverySecond, _spawnEnemyEverySecond);	//Start spawning enemies
	}

	public void InvokeAsteroids () {	//Spawns asteroids until max spawn amount has been reached
		CancelInvoke("SpawnAsteroids");
		InvokeRepeating("SpawnAsteroids", _asteroidSpawnDelay, _asteroidSpawnDelay);
	}

	public void AddPoints (int points ) {						//Adds points to score
		_toPoints+=points*_scoreMultiplier;
	}

	public void SetScoreMultiplier (int amount ) {
		_scoreMultiplier = amount;
	}

	public void AddScoreMultiplier (float amount ) {		//calculate score
        if (ThirdManager.isTutorial)
            return;
		if(_scoreMultiplier < _maxScoreMultiplier){
			this.AddPoints(1);
			score += (int)(100*amount);
			GUIManager.instance.ShowScore(score);
		}
	}

	public void CountPoints() {								//Adds points to score
		if(_toPoints > _score){
			_score+= (int)Mathf.Ceil((_toPoints - _score)*.1f);

		}
	}

	public void Drop ( Vector3 pos) {							//Drop an item on the play field
		if(Player.instance._inControl)					//Won't drop anything if player has just died
			Instantiate(_pickup, pos , _pickup.transform.rotation);
	}

	// Calculates the game area based on camera edges by casting rays from all corners of the main camera
	public void CalcBounds () {
		if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0, 0, 0)),out _bottomLeft)){					
			_bottomLeft.point=new Vector3(_bottomLeft.point.x,0f,_bottomLeft.point.z); 
		}		
		if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(1, 0, 0)),out _bottomRight)){
			_bottomRight.point=new Vector3(_bottomRight.point.x,0f,_bottomRight.point.z); 
		}
		if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(1, 1, 0)),out _topRight)){
			_topRight.point=new Vector3(_topRight.point.x,0f,_topRight.point.z);
		}
		if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0, 1, 0)),out _topLeft)){
			_topLeft.point=new Vector3(_topLeft.point.x,0f,_topLeft.point.z);
		}
		//Set game are area
		_gameWidth = Vector3.Distance(_bottomLeft.point, _bottomRight.point);
		_gameHeight = Vector3.Distance(_bottomLeft.point, _topLeft.point);
		//Calculate half sizes once to avoid doing this multiple times in other voids
		_gameWidthHalf = _gameWidth*.5f +.75f;
		_gameHeightHalf = _gameHeight*.5f +.75f;
	}

	//Positiones the 3D GUI
	void GUIPosition () {	
		//_guiBottomLeft.transform.position = _bottomLeft.point;
	}

	//Visuals to be able to see the play area and other information in the editor
	void OnDrawGizmos () {
		if(!Application.isPlaying)									//Only calculate repeatedly when not in play mode
			CalcBounds();
			Gizmos.DrawCube(_bottomLeft.point, Vector3.one*.1f);			//Draw cubes on game area corners (without padding)
			Gizmos.DrawCube(_bottomRight.point, Vector3.one*.1f);	
			Gizmos.DrawCube(_topRight.point, Vector3.one*.1f);
			Gizmos.DrawCube(_topLeft.point, Vector3.one*.1f); 		
		 	Gizmos.DrawWireCube (transform.position, new Vector3 (_gameWidth,0,_gameHeight));	//Draw wires for padded game area
	}

	//Check if a gameObject is outside the game area (this is run by objects that wraps edges when leaving area)
	//CheckBounds (	obj: 	object to wrap, 
	//				ps: 	particle system to disable then re enable, (world particle systems sometimes leaves a trail when moved instantly)(set to null if no particle system)
	public void CheckBounds (GameObject obj , ParticleSystem ps) { 	
		float x0=obj.transform.position.x;
		float z0=obj.transform.position.z;
		if(obj.transform.position.x > _gameWidthHalf){	
			x0 = -_gameWidthHalf ;
			if(ps)DisableEnablePS(ps);
		}else if(obj.transform.position.x < -_gameWidthHalf){
			x0 = _gameWidthHalf;
			if(ps)DisableEnablePS(ps);
		}
		if(obj.transform.position.z > _gameHeightHalf){	
			z0 = -_gameHeightHalf;
			if(ps)DisableEnablePS(ps);
		}else if(obj.transform.position.z < -_gameHeightHalf){
			z0 = _gameHeightHalf;
			if(ps)DisableEnablePS(ps);
		}
		obj.transform.position=new Vector3(x0,obj.transform.position.y,z0);
	}
	//padding: some objects like pickups need to be further inside the screen to be visible and touchable/clickable, added some padding to fix
	public void CheckBoundsInverted (GameObject obj , ParticleSystem ps, float padding ) { 	

		float x0=obj.transform.position.x;
		float z0=obj.transform.position.z;

		if(obj.transform.position.x > _gameWidthHalf+-padding){	
			x0 = _gameWidthHalf -padding;
			if(ps) DisableEnablePS(ps);
		}else if(obj.transform.position.x < -_gameWidthHalf+padding){
			x0 = -_gameWidthHalf +padding;
			if(ps) DisableEnablePS(ps);
		}
		if(obj.transform.position.z > _gameHeightHalf-padding){	
			z0 = _gameHeightHalf -padding;
			if(ps )DisableEnablePS(ps);
		}else if(obj.transform.position.z < -_gameHeightHalf+padding){
			z0 = -_gameHeightHalf +padding;
			if(ps) DisableEnablePS(ps);
		}
		obj.transform.position=new Vector3(x0,obj.transform.position.y,z0);
	}
	//disables then adds delay to re enable, fixes particle trails
	void DisableEnablePS(ParticleSystem ps){
		if(ps.enableEmission){
			ps.enableEmission = false;
			StartCoroutine(DisableEnableYield(ps));
	 	}
	}

	IEnumerator DisableEnableYield(ParticleSystem ps){
		yield return new WaitForSeconds(0.05f);
		if(ps && ps.transform.parent.GetComponent<Collider>().enabled)
			ps.enableEmission = true;
	}
	//destroy all gameobjects with tag (Use with causion, Find is heavy on mobile)
	void DestroyAll(string tag) {
		GameObject[] gameObjects  =  GameObject.FindGameObjectsWithTag (tag);
	    for(int i = 0 ; i < gameObjects.Length ; i ++)
	        Destroy(gameObjects[i]);
	}

	//Create a force explosion
	void Explosion (Vector3 explosionPos , float radius ,  float power) {
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);     
		foreach ( Collider hit in colliders) {
            if (!hit)
                continue;           
            if (hit.GetComponent<Rigidbody>() && hit.gameObject.tag!="Bullet")
                hit.GetComponent<Rigidbody>().AddExplosionForce(power , explosionPos, radius);
        }
	}


	public void GameOver (bool levelComplete ) {

		SoundController.instance.PlayMusic(null, 1.5f, 1, true);				//Fade down the music
		StartCoroutine(GameOverWait(levelComplete));
	}

	IEnumerator GameOverWait(bool levelComplete){ //if levelComplete=true: Lose,  false:Win
		yield return new WaitForSeconds(2f);
        if (ThirdManager.isTutorial)
			ThirdManager.instance.LoadScene ("Main");
        else
        {
            SaveStats.instance._score = score;								//Saves score
            SaveStats.instance._previousLevel = Application.loadedLevelName;	//Saves this levels name so that it can be used in a replay button
            SaveStats.instance._levelComplete = levelComplete;					//Did the player die or complete the level
            if (!levelComplete) //win
            {
                GameData.SetCash(GUIManager.instance.coin);
            }

            if (SaveStats.instance._score > PlayerPrefs.GetInt("bestScore", 0))
            {
                PlayerPrefs.SetInt("bestScore", SaveStats.instance._score);
                PlayerPrefs.SetInt("totalScore", SaveStats.instance._score + PlayerPrefs.GetInt("totalScore", 0));
            }
			ThirdManager.instance.LoadScene ("Menu");								//Load game over scene
        }
		
	}
}
