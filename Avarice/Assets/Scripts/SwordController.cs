using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    public static SwordController instance;
    [SerializeField]
    private static float fStunDuration = 2.0f;
    

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other) {
       if (other.CompareTag("Enemy")) {
            // Stun enemy
            Debug.Log("Hit");
            other.GetComponent<EnemyAI>().StunEnemy(fStunDuration);
       }
    }

    //private void OnCollisionEnter(Collision other) {
    //    if (other.gameObject.CompareTag("Enemy") && PlayerController.bIsAttacking) {
    //        // Stun enemy
    //        Debug.Log("Hit");
    //        other.gameObject.GetComponent<EnemyAI>().StunEnemy(fStunDuration);
    //    }
    //}

}
