using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public GameObject player;
    public NavMeshAgent agent;

    private bool bIsStunned = false;

    private Ray ray;
    private GameObject coin;

    public GameObject FindClosestCoin()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Distraction");
        GameObject closest = null;
        float distance = 100000.0f;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }



    public GameObject FindPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = 100000.0f;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    // Use this for initialization
    void Start () {
        player = FindPlayer();
        StunEnemy(1.0f);
	}

    // Update is called once per frame
    void Update () {

        if (bIsStunned) {
            return;
        }

        ray.origin = player.transform.position;
        ray.direction = Vector3.down;

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
        }

        coin = FindClosestCoin();

        if(coin != null)
        {
            ray.origin = coin.transform.position;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }

            coin = null;
        }

        

    }

    public void StunEnemy(float _fDuration) {
        if (bIsStunned) {
            return;
        }
        bIsStunned = true;
        agent.isStopped = true;
        StartCoroutine(RemoveStun(_fDuration));
    }

    private IEnumerator RemoveStun(float _fDuration) {
        yield return new WaitForSeconds(_fDuration);
        bIsStunned = false;
        agent.isStopped = false;
    }

}
