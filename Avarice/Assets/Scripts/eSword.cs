using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eSword : MonoBehaviour {

    public int iDam = 10;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DamagePlayer(iDam);
            Debug.Log("EggBoy");
            
        }
    }

}
