using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{
    public float damage;
    public Transform target;
    private NavMeshAgent agent;
    private Animator anim;
    public float attackDistance;
    public float attackDamage = 3;
    public float attackTimeout = 4;
    public float exploreRange = 4;
    private bool targetIsBoy;
    public EnemySpawner mySpawner;
    public Vector3 mySpawnSpot;

    public float defaultSpeed, scaredSpeed, scaredTime = 3;
    public bool isScared;

    float scRemaining;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }

    public void InitAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }

    public void Scatter(Transform awayFrom , float fleeDistance)
    {
        Vector3 toPlayer = awayFrom.position - transform.position;

        Vector3 inverted = toPlayer * -1;

        inverted *= fleeDistance;

        Vector3 desiredFleeLocation = transform.position + inverted;
        isScared = true;
        agent.speed = scaredSpeed;

        agent.SetDestination(desiredFleeLocation);
        scRemaining = scaredTime;
    }

    private void Update()
    {
        anim.SetBool("walking", agent.speed == defaultSpeed && agent.remainingDistance > 0.5f);
        anim.SetBool("running", agent.speed == scaredSpeed);
        if (isScared)
        {
            scRemaining -= Time.deltaTime;

            if(scRemaining <= 0)
            {
                // not scared no mo
                isScared = false;
                agent.speed = defaultSpeed;
                UpdateTarget();
            }
        }


        if (target)
        {
            Vector3 dir = target.transform.position - transform.position;
            if (dir.magnitude <= attackDistance && CanAttack)
            {
                // do attack
                lastAttackTime = Time.time;
                DoAttack(target);
            }
        }
    }

    private bool CanAttack
    {
        get { return Time.time - lastAttackTime > attackTimeout; }
    }
    public float lastAttackTime;

    public void UpdateTarget(bool toSpawnSpot = false)
    {
        if (toSpawnSpot)
        {
            target = null;
            agent.SetDestination(mySpawnSpot);
            return;
        }
        else
        {
            if (target == null || targetIsBoy != Player.Instance.hasDragon) GetTarget();

            if (target == null) return;

            if (!isScared)
                agent.SetDestination(Vector3.Distance(target.position, mySpawnSpot) < exploreRange ? target.transform.position : mySpawnSpot);
        }
    }

    public void GetTarget()
    {
        agent.speed = defaultSpeed;
        targetIsBoy = Player.Instance.hasDragon || (!mySpawner.dragonInArea && mySpawner.playerInArea);

        Transform newTarget = targetIsBoy ? Player.Instance.transform : Player.Instance.dragon;
        if (newTarget != null)
        {
            target = newTarget;
        }
    }

    void DoAttack(Transform toHit)
    {
        anim.SetTrigger("attack");
        if (targetIsBoy)
            Player.Instance.StartCoroutine("TakeHit"); // Knock back player
        else
            Player.Instance.DragonTakeDamage(attackDamage); // Damage to dragon
    }
}
