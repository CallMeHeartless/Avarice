using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public GameObject player;
    public NavMeshAgent agent;
    public float fAttackRadius = 2.0f;

    private bool bIsStunned = false;
    private bool bIsAttacking = false;
    private float fAttackRate = 0.6f;

    private Ray ray;
    private GameObject coin;
    private Animator anim;
    private float fDistance;

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

    private void Attack()
    {
        bIsAttacking = true;
        // Animation
        anim.SetTrigger("Attack");
        // cooldown
        StartCoroutine(AttackCooldown(fAttackRate));
    }

    IEnumerator AttackCooldown(float _fAttackCooldown)
    {
        yield return new WaitForSeconds(_fAttackCooldown);
        bIsAttacking = false;
        anim.SetTrigger("Run");
    }

    void AttackDistance()
    {
        fDistance = (player.transform.position - transform.position).magnitude;
        
        if(fDistance < fAttackRadius)
        {
            Attack();
        }
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
        StunEnemy(2.0f);
        anim = GetComponentInChildren<Animator>();
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

        if(coin == null)
        {
            AttackDistance();
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
