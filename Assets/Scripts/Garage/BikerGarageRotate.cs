using UnityEngine;
using System.Collections;

public class BikerGarageRotate : MonoBehaviour {

	private Transform tr;
	public Transform motoTr;
	// Use this for initialization
	[HideInInspector]
	public GameObject[] bikeList;
	public static BikerGarageRotate instance;

	void Awake(){
		instance=this;
	}

	void Start () {
		tr = transform;
		bikeList=new GameObject[7];
		GameObject obj;
		for(int i=0;i<7;i++)
		{
			obj = (GameObject)Instantiate(Resources.Load<GameObject>("Prefab/Player"+i.ToString()));
//			obj.GetComponent<Tank>().isGarage=true;
			obj.SetActive(false);
			obj.transform.parent=motoTr;
			obj.transform.localPosition=Vector3.zero;
			obj.transform.localScale=new Vector3(6,6,6);
			bikeList[i]=obj;
		}
	}
	
	// Update is called once per frame
	void Update () {
		tr.Rotate(new Vector3(0,-50*Time.deltaTime,0));
	}
}
