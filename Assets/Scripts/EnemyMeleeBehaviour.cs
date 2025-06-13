using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform Player;
    public LayerMask whatIsGround, whatIsPlayer;
    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //Attacking
    // public float timeBetweenAttacks;
    // bool alreadyAttacked;
    public float EnemyHealth;
    //States
    public float sightRange;
    public bool playerInSight;
    public NavMeshAgent agent;

    private void Angered()
    {
        Player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        //Check if player in sight
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if (!playerInSight)
        {
            Patrolling();
        }
        else ChasePlayer();
    }
    private void Patrolling()
    {
        if (!walkPointSet)
            SearchWalkPoint();
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
    }
    private void ChasePlayer()
    {
        transform.LookAt(Player);
        agent.SetDestination(Player.position);
    }
    public void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
