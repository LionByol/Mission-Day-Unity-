using UnityEngine;
using System.Collections;

public abstract class PagingStorageSlot : MonoBehaviour {
	
	// Item gid 
	abstract public int gid { get; set; }
	
	// 重新填写数据 
	abstract public void ResetItem();
}
