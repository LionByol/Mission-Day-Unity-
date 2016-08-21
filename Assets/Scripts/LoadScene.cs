using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {
    int cn = 0;
	
	void Update () {
        cn++;
        if (cn > 10)
            NextScene();
	}

    void NextScene()
    {
            ThirdManager.instance.LoadScene("Main");
    }
}
