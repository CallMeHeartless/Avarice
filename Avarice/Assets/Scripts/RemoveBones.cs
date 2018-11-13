using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBones : MonoBehaviour {

    private float fTime = 0.0f;
    private float fTarTime = 30.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(fTime >= fTarTime)
        {
            Destroy(gameObject);
        }
        else
        {
            fTime = fTime + Time.deltaTime;
        }
	}
}
