using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public GameObject player;
    public NavMeshAgent agent;
    public float fAttackRadius = 2.0f;

    public GameObject[] Patrolpoints;
    public int PatrolLength = 3;
    private int CurrPatrol;
    private bool bDecision = false;

    private bool bIsStunned = false;
    private bool bIsAttacking = false;
    private float fAttackRate = 0.6f;
    private bool bCanAttack = true;

    private Ray ray;
    private GameObject coin;
    public Animator anim;
    private float fDistance;

    public bool bPursue = false;

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
        anim.SetTrigger("Run");
        bIsAttacking = false;

    }

    void AttackDistance()
    {
        fDistance = (player.transform.position - transform.position).magnitude;

        if (fDistance < fAttackRadius)
        {
            if (bCanAttack == true)
            {
                Attack();
            }

        }
    }

    public void SetPatrolPoints()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Level1Patrol");
        for (int i = 0; i < Patrolpoints.Length; i++)
        {
            int Rand = Random.Range(0, gos.Length);
            Patrolpoints[i] = gos[Rand];
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

    public void SetPatrolPoint()
    {
        CurrPatrol = Random.Range(0, Patrolpoints.Length);
    }

    public void patrol()
    {
        ray.origin = Patrolpoints[CurrPatrol].transform.position;
        ray.direction = Vector3.down;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
        }

        if((transform.position.x == Patrolpoints[CurrPatrol].transform.position.x) && (transform.position.z == Patrolpoints[CurrPatrol].transform.position.z) && !bDecision)
        {
            StartCoroutine(PatrolAgain());
            bDecision = true;
            anim.SetTrigger("Hit");
        }
    }

    public IEnumerator PatrolAgain()
    {
        yield return new WaitForSeconds(2);
        SetPatrolPoint();
        bDecision = false;
        anim.SetTrigger("Recover");
    }

    public void movement()
    {
        ray.origin = player.transform.position;
        ray.direction = Vector3.down;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
        }

        coin = FindClosestCoin();

        if (coin != null)
        {
            ray.origin = coin.transform.position;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }

            coin = null;
        }

        if (coin == null)
        {
            AttackDistance();
        }
    }

    // Use this for initialization
    void Start () {
        player = FindPlayer();
        //StunEnemy(2.0f);
        anim = GetComponentInChildren<Animator>();
        Patrolpoints = new GameObject[PatrolLength];
        SetPatrolPoints();
        SetPatrolPoint();
    }

    // Update is called once per frame
    void Update () {

        if (bIsStunned) {
            return;
        }
        else if(bPursue)
        {
            movement();
        }
        else
        {
            patrol();
        }
        

    }

    public void StunEnemy(float _fDuration) {
        if (bIsStunned) {
            return;
        }
        bIsStunned = true;
        bCanAttack = false;
        agent.isStopped = true;
        anim.SetTrigger("Hit");
        StartCoroutine(RemoveStun(_fDuration));
    }

    private IEnumerator RemoveStun(float _fDuration) {
        yield return new WaitForSeconds(_fDuration);
        bIsStunned = false;
        bCanAttack = true;
        agent.isStopped = false;
        anim.SetTrigger("Recover");
    }

}
