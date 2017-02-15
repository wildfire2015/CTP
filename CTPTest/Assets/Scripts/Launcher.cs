using UnityEngine;
using System.Collections;
using PSupport;
using PSupport.MTLoadSystem;
using PSupport.PSingleton;

public class Launcher : MonoBehaviour {

    LoadInfo mloadinfo = null;

    // Use this for initialization
    void Start ()
    {
        mloadinfo = FindObjectOfType<LoadInfo>();
        MTResourceLoadManager o = MTResourceLoadManager.instance;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
