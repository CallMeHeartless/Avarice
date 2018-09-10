using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public GameObject player;
    public NavMeshAgent agent;

    private Ray ray;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        ray.origin = player.transform.position;
        ray.direction = Vector3.down;

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
        }
		
	}
}
