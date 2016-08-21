using UnityEngine;
using System.Collections;

public class SpinManager : MonoBehaviour {

    public UISprite[] _images;
    public UISprite _outline;
    public GameObject mainPanel;

    private bool isClicked = true;
    string[] images = {"200", "100", "50"};
    const string defaultImageName = "_img";
    string[] spinImages = { defaultImageName, defaultImageName, defaultImageName, defaultImageName, defaultImageName, defaultImageName };

    void Start()
    {
        Init();
    }

    void Init()
    {
        int i = 0;
        int c = 0;
        while (i < 3 && c<100)
        {
            int k = Random.Range(0, 6);

            if (_images[k] != null)
            {
                if (spinImages[k] == defaultImageName)
                {
                    spinImages[k] = images[i];
                    i++;
                }
            }
            else
            {
                return;
            }
            c++;
        }
    }

    public void FlipImage(string value)
    {
        int i = int.Parse(value);
        Debug.Log(i);
        if (isClicked)
        {
            isClicked = false;

            _images[i].spriteName = getSpin(i);
            Vector3 pos0=_images[i].transform.localPosition;
            _outline.transform.localPosition= new Vector3(pos0.x+3,pos0.y,pos0.z);
            if (_images[i].spriteName != defaultImageName)
            {
                _images[i].SetDimensions(80, 150);
                _images[i].GetComponent<UITweener>().enabled=true;
            }
            else
            {
                _images[i].gameObject.SetActive(false);
            }
            StartCoroutine(FlipAll());
        }
    }

    public void closeDialog()
    {
        mainPanel.SetActive(false);
    }

    public string getSpin(int i)
    {
        return spinImages[i];
    }

    IEnumerator FlipAll()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < 6; i++)
        {
            _images[i].spriteName = spinImages[i];
            if(spinImages[i]!=defaultImageName)
            {
                _images[i].SetDimensions(80, 150);
            }
            else
            {
                _images[i].gameObject.SetActive(false);
            }
        }
    }
}
