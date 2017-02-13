using UnityEngine;
using System.Collections;
using PSupport;
using PSupport.MTLoadSystem;

public class Launcher : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        MTResourceLoadManager o = MTResourceLoadManager.instance;
        Debug.Log(o.a);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
