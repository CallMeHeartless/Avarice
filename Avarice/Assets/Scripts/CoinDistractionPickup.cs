using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDistractionPickup : MonoBehaviour {
    private bool bCollision = false;
	
    public void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Enemy")) {

            DespawnTimer();

            
        }
    }

    public void DespawnTimer()
    {
        if (!bCollision)
        {
            bCollision = true;
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

}
