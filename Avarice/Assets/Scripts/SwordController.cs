using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    public int iDamage = 1;

    public static SwordController instance;
    [SerializeField]
    private static float fStunDuration = 2.0f;
    [SerializeField]
    private AudioSource attackAudio;
    

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other) {
       if (other.CompareTag("Enemy") && PlayerController.bIsAttacking) {
            // Stun enemy
            other.GetComponent<EnemyAI>().iHealth = other.GetComponent<EnemyAI>().iHealth - iDamage;
            if(other.GetComponent<EnemyAI>().iHealth > 0)
            {
                other.GetComponent<EnemyAI>().StunEnemy(fStunDuration);
            }
            attackAudio.Play();
            
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
