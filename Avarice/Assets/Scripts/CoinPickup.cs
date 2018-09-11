using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    public int iValue = 1;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().AddCoinsToInventory(iValue);
            // Noise

            Destroy(gameObject);
        }
    }


}
