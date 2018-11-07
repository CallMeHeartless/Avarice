using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassTargetController : MonoBehaviour {

    [SerializeField]
    private Transform target;

	// Use this for initialization
    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerController.SetCompassTarget(target);
        }
    }
}
