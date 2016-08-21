public var LightIntensityMult:float = -0.5f;
public var LifeTime:float = 1;
public var RandomRotation:boolean = false;
public var PositionOffset:Vector3;

function Start(){

	this.gameObject.transform.position += PositionOffset;
	if(RandomRotation){
		this.gameObject.transform.rotation.x = Random.rotation.x;
		this.gameObject.transform.rotation.y = Random.rotation.y;
		this.gameObject.transform.rotation.z = Random.rotation.z;
	} 
	GameObject.Destroy(this.gameObject,LifeTime);
	
}
function Update () {
	if(this.gameObject.GetComponent.<Light>()){
		this.GetComponent.<Light>().intensity += LightIntensityMult * Time.deltaTime;
	}
}