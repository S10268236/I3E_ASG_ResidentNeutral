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
    public float MaxEnemyHealth;
    [SerializeField]
    FloatingHealthBar healthBar;
    //Time between walks
    private float walkTime = 0f;
    //States
    public float sightRange;
    public bool playerInSight;
    public NavMeshAgent agent;
    [SerializeField]
    GameObject MutagenLoot;
    [SerializeField]
    Transform LootSpawn;
    private bool isLooted = false;

    private void Awake()
    {
        Player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.UpdateHealthBar(EnemyHealth, MaxEnemyHealth);
    }
    void Start()
    {
        EnemyHealth = MaxEnemyHealth;
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
            walkTime += Time.deltaTime;
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f || walkTime >= 5f)
        {
            walkPointSet = false;
            walkTime = 0f;
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
    public void TakeDamage(float damage)
    {
        EnemyHealth -= damage;
        Debug.Log("Enemy:" + EnemyHealth);
        Debug.Log("Max: " + MaxEnemyHealth);
        healthBar.UpdateHealthBar(EnemyHealth, MaxEnemyHealth);
        if (EnemyHealth <= 0 && !isLooted)
        {
            isLooted = true;
            Invoke(nameof(Loot), 0.5f);
        }
    }
    private void Loot()
    {
        Destroy(gameObject);
        GameObject newMutagenLoot = Instantiate(MutagenLoot, LootSpawn.position, LootSpawn.rotation);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
