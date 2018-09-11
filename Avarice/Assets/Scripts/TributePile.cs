using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TributePile : MonoBehaviour {

    private bool bHasPlayer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(bHasPlayer && Input.GetKeyDown(KeyCode.E)) {
            GameManager.AddCoinsToTribute(PlayerController.PayTribute());
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            bHasPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            bHasPlayer = false;
        }
    }
}
