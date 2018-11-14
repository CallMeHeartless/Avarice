﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public int iHealth = 30;
    private bool bIsAlive = true;

    public GameObject Sword;
    public Collider SwordCollider;

    public GameObject player;
    public NavMeshAgent agent;
    public float fAttackRadius = 2.0f;

    public GameObject[] Patrolpoints;
    public int PatrolLength = 3;
    private int CurrPatrol;
    private bool bDecision = false;

    public bool bIsStunned = false;
    private bool bIsAttacking = false;
    private float fAttackRate = 0.6f;
    private bool bCanAttack = true;
    public bool bHeavyAttack = false;
    public bool bHeavyAttacking = false;

    private Ray ray;
    private GameObject coin;
    public Animator anim;
    private float fDistance;

    public bool bPursue = false;

    public void EnableSword()
    {
        SwordCollider.enabled = true;
    }

    public void DisableSword()
    {
        SwordCollider.enabled = false;
    }

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

    public void ChooseAttack()
    {
        int i = Random.Range(1, 5);

        if(i == 1)
        {
            bHeavyAttack = true;
        }
        else
        {
            bHeavyAttack = false;
        }
    }

    private void HeavyAttack()
    {
        /*
        if((agent.remainingDistance <= 15.0f) && !bHeavyAttacking)
        {
            agent.stoppingDistance = 15.0f;
            bHeavyAttacking = true;
            anim.SetTrigger("Idle");
            StartCoroutine(Lunge(2.0f));

        }
        */

        if ((agent.remainingDistance <= 15.0f) && !bHeavyAttacking)
        {
            Vector3 targetDir = player.transform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(targetDir);

            agent.stoppingDistance = 15.0f;
            
            bHeavyAttacking = true;
            
            bIsAttacking = true;
            // Animation
            anim.SetTrigger("HeavyAttack");
            // cooldown
        }
    }

    public void Lunge(float _LungeTimer)
    {
        agent.enabled = true;
        agent.angularSpeed = 300.0f;
        agent.stoppingDistance = 1.0f;
        agent.acceleration = 40.0f;
        agent.angularSpeed = 40.0f;
        agent.speed = 20.0f;
        bIsAttacking = true;
        StartCoroutine(AttackCooldown(fAttackRate));
    }

    private void Attack()
    {
        agent.enabled = false;
        bIsAttacking = true;
        // Animation
        anim.SetTrigger("Attack02");
        // cooldown
        StartCoroutine(AttackCooldown(fAttackRate));
    }

    IEnumerator AttackCooldown(float _fAttackCooldown)
    {
        yield return new WaitForSeconds(_fAttackCooldown);
        if(bIsAlive)
        {
            SwordCollider.enabled = false;
            agent.enabled = true;
            anim.SetTrigger("Run");
            bIsAttacking = false;
            bHeavyAttacking = false;
            agent.angularSpeed = 120.0f;
            agent.acceleration = 8.0f;
            agent.speed = 4.0f;
            ChooseAttack();
        }
    }

    void AttackDistance()
    {
        fDistance = (player.transform.position - transform.position).magnitude;

        if(bHeavyAttack && bPursue)
        {
            SwordCollider.enabled = true;
            HeavyAttack();
        }
        else
        {
            if ((fDistance < fAttackRadius) && bPursue)
            {
                if (bCanAttack == true)
                {
                    SwordCollider.enabled = true;
                    Attack();
                }

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
            anim.SetTrigger("Idle");
        }
    }

    public IEnumerator PatrolAgain()
    {
        yield return new WaitForSeconds(2);
        SetPatrolPoint();
        bDecision = false;
        anim.SetTrigger("Run");
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

        CoinMovement();

        if (coin == null)
        {
            if(bPursue == true)
            {
                AttackDistance();
            }
        }
    }

    public void CoinMovement()
    {
        coin = FindClosestCoin();

        RaycastHit hit;

        if (coin != null)
        {
            ray.origin = coin.transform.position;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }

            coin = null;
        }
    }

    // Use this for initialization
    void Start () {
        player = FindPlayer();
        ChooseAttack();
        //StunEnemy(2.0f);
        anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("Run");
        Patrolpoints = new GameObject[PatrolLength];
        SetPatrolPoints();
        SetPatrolPoint();
        SwordCollider = Sword.GetComponent<SphereCollider>();
        SwordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update () {

        if (agent.enabled)
        {
            if (bIsStunned)
            {
                return;
            }
            else if (bPursue)
            {
                movement();
            }
            else
            {
                patrol();
            }
        }
        Death();
    }

    public void Death()
    {
        if(iHealth <= 0 && bIsAlive)
        {
            bIsAlive = false;
            agent.enabled = false;
            anim.SetTrigger("Death");
            StartCoroutine(Despawn());
        }
    }

    public IEnumerator Despawn()
    {
        yield return null;
        int r = Random.Range(1, 5);
        if(r >= 3 )
        {
            GameObject coin = Instantiate(Resources.Load("Coin Pickup", typeof(GameObject))) as GameObject;
            coin.transform.position = transform.position;
        }
        GameObject ded = Instantiate(Resources.Load("SkellyDeath", typeof(GameObject))) as GameObject;
        ded.transform.position = transform.position;
        ded.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }

    public void StunEnemy(float _fDuration) {
        if (bIsStunned) {
            return;
        }

        if(iHealth > 0)
        {
            bIsStunned = true;
            bCanAttack = false;
            SwordCollider.enabled = false;
            agent.enabled = false;
            anim.ResetTrigger("Attack02");
            anim.SetTrigger("Hit");
            StartCoroutine(RemoveStun(_fDuration));
        }
        
    }

    private IEnumerator RemoveStun(float _fDuration) {
        yield return new WaitForSeconds(_fDuration);
        if(iHealth > 0)
        {
            bIsStunned = false;
            bCanAttack = true;
            agent.enabled = true;
            anim.SetTrigger("Idle");
        }
    }

    public void DamageEnemy(int _iDamage) {
        iHealth -= _iDamage;
    }

}
