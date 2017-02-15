using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadInfo : MonoBehaviour {
    
    private Image mImgRate = null;
    private Text mTxtRate = null;
	// Use this for initialization
	void Start ()
    {
        mImgRate = gameObject.transform.Find("Img_rate").GetComponent<Image>();
        mTxtRate = gameObject.transform.Find("Txt_rate").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    public void setRate(float rate)
    {
        float frate = rate * 100;
        float showrate = frate > 100 ? 100 : frate;
        mTxtRate.text = showrate.ToString("0") + "%";
        mImgRate.fillAmount = rate;
    }
}
