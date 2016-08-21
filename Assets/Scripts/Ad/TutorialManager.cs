using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {
	public static TutorialManager instance;    
	/*public UILabel note;*/
	public GameObject[] animation;	
	[HideInInspector]
	public int step=-1;

    public UILabel timeLabel;
    public GameObject timeObj;

	void Awake(){
		instance=this;
	}
	// Use this for initialization
	void Start () {
        step = 0;
        if (ThirdManager.isTutorial)
        {
            animation[0].SetActive(true);
            StartCoroutine(ShowTime());
		}
	}

    IEnumerator ShowTime()
    {
        int sec = 30;
        timeObj.SetActive(true);
        while (sec>0)
        {
            if (sec < 0)
                sec = 0;
            int minute = sec / 60 % 60;
            int second = sec % 60;
            timeLabel.text = string.Format("{0:D1}", minute) + ":" + string.Format("{0:D2}", second);
            yield return new WaitForSeconds(1.0f);
            sec--;
        }
//        Application.LoadLevel("Main");
		ThirdManager.instance.LoadScene ("Main");
    }

    public void NextTutorial()
    {
       // print(step);
        if (ThirdManager.isTutorial) {
            animation[step].SetActive(false);
            StartCoroutine(IncreaseStep());
        }
	}

    IEnumerator IncreaseStep()
    {
        ThirdManager.isTutorial = false;
        yield return new WaitForSeconds(1f);
        step++;
        if (step < 3)
            animation[step].SetActive(true);
        ThirdManager.isTutorial = true;
    }
    public void CheckTap()
    {
        if (step == 2)
            NextTutorial();
    }

    public void CheckSwip()
    {
        if (step == 0 || step==1)
            NextTutorial();
    }
}
