using UnityEngine;
using System.Collections;
using System.IO;
using System;

public struct BikeData
{
	public int id;
	public string level;
	public int price;
	public int speed;
	public float health;
	public int force;
}

public struct LevelData
{
	public int id;
	public string ranks;
	public int score;
	public int greyStone;
	public int blueStone;
	public int redStone;
	public int ufo1;
	public int ufo2;
	public int ufo3;
}

public class LoadData : MonoBehaviour {

	public static Hashtable BikeList =  new Hashtable();	
	public static Hashtable LevelList =  new Hashtable();	

	static public string	localDir;

	void Awake()
	{
		localDir	="Data/";// GetCachePath();		
	}

	void Start () {
		LoadBikeData();
		LoadLevelData();
	}    
	
	public static void LoadBikeData()	
	{
		BikeList.Clear();
		string[][] str_array = GetStringFromMBText(localDir+"BikeData",true);
		print ("count:"+str_array[0].Length);
		for(int i=0;i<str_array[0].Length;i++)
		{
			BikeData bike; 
			bike.id =  int.Parse(CheckNullStr(str_array[0][i]));
			bike.level=CheckNullStr(str_array[1][i]);
			bike.price=int.Parse(CheckNullStr(str_array[2][i]));
			bike.speed=int.Parse(CheckNullStr(str_array[3][i]));
			bike.health=float.Parse(CheckNullStr(str_array[4][i]));
			bike.force=int.Parse(CheckNullStr(str_array[5][i]));
//			print (bike.id+":");
			BikeList.Add(bike.id,bike);	
		}
	}

	public static void LoadLevelData()	
	{
		LevelList.Clear();
		string[][] str_array = GetStringFromMBText(localDir + "LevelData",true);
		for(int i=0;i<str_array[0].Length;i++)
		{
			LevelData level;
			level.id =  int.Parse(CheckNullStr(str_array[0][i]));
			level.ranks=CheckNullStr(str_array[1][i]);
			level.score=int.Parse(CheckNullStr(str_array[2][i]));
			level.greyStone=int.Parse(CheckNullStr(str_array[3][i]));
			level.blueStone=int.Parse(CheckNullStr(str_array[4][i]));
			level.redStone=int.Parse(CheckNullStr(str_array[5][i]));
			level.ufo1=int.Parse(CheckNullStr(str_array[6][i]));
			level.ufo2=int.Parse(CheckNullStr(str_array[7][i]));
			level.ufo3=int.Parse(CheckNullStr(str_array[8][i]));

			LevelList.Add(level.id,level);			
		}
	}

	public static string CheckNullStr(string str)
	{
		string return_str = str;
		if(str == null)
			return_str = "0";
		if(str == "")
			return_str = "0"; 
		return return_str;
	}
	

	public static string[][] GetStringFromMBText(string mb_name,bool Filled)
	{
		TextAsset text_asset = (TextAsset)Resources.Load<TextAsset>(mb_name);
		print ("FromFile:" + mb_name);
		byte[]	dat			= (byte[])text_asset.bytes.Clone();
        string [][] str_array = MBDeal.readMBFromBytes(dat,false,Filled);
		return str_array;
	}

	public static string GetCachePath()
	{
		string cachePath="";
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			cachePath = Application.persistentDataPath+"/";
		}
		else if (Application.platform == RuntimePlatform.Android) {
			cachePath = Application.persistentDataPath+"/";
		}
		else {
			cachePath =  "cache/";
		}
		
		return cachePath;
	}
}