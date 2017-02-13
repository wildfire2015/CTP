using UnityEngine;
using System.Collections;
using PSupport;
using PSupport.MTLoadSystem;

public class Launcher : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        MTResourceLoadManager o = MTResourceLoadManager.instance;
        //MTResourceLoadManager.ReleaseInstance();
        o = MTResourceLoadManager.instance;
        //MTResourceLoadManager.ReleaseInstance();
        o = MTResourceLoadManager.instance;
        //MTResourceLoadManager.ReleaseInstance();
        o = MTResourceLoadManager.instance;
        //MTResourceLoadManager.ReleaseInstance();
        o = MTResourceLoadManager.instance;

        testsinglemono tm = testsinglemono.instance;
        testsinglemono.ReleaseInstance();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
