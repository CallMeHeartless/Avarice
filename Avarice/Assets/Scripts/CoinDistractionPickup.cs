using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDistractionPickup : MonoBehaviour {

	
    public void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Enemy")) {
            Destroy(gameObject);
        }
    }

}
