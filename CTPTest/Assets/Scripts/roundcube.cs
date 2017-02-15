using UnityEngine;
using System.Collections;

public class roundcube : MonoBehaviour {
    private Vector3 mV3Routation = new Vector3();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //mV3Routation.x += Time.deltaTime * 180;
        mV3Routation.y += Time.deltaTime * 180;
        //mV3Routation.z += Time.deltaTime * 180;
        Quaternion q =  Quaternion.Euler(mV3Routation);
        gameObject.transform.localRotation = q;


    }
}
