using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private EnemyMovement enemyMovement;

    [Header("Ranges")]
    //public float attackRange = 10f;        // Distance max pour tirer
    public float retreatDistance = 5f;     // Distance à partir de laquelle il fuit

    [Header("Attack")]
    public float attackCooldown = 2f;
    public int damage = 10;
    private float attackTimer;
    //private float sqrAttackRange;
    private float sqrRetreatDistance;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float speed = 20f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyMovement = GetComponent<EnemyMovement>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'Player' trouvé !");
        }
    }

    void Start()
    {
        //sqrAttackRange = attackRange * attackRange;
        sqrRetreatDistance = retreatDistance * retreatDistance;
    }

    void Update()
    {
        if (player == null) return;
        if (!enemyMovement.firstDetection) return;

        float sqrDistance = (player.position - transform.position).sqrMagnitude;

        // PRIORITÉ : fuite si trop proche
        if (sqrDistance < sqrRetreatDistance)
        {
            Flee();
            return; // IMPORTANT : empêche l'attaque
        }

        // Ensuite : attaque si dans range
        if (enemyMovement.HasLineOfSight(player.position + Vector3.up * 0.6f - transform.position))
        {
            StopAndShoot(sqrDistance);
            return;
        }

        // Sinon : il s'approche
        Chase();
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void StopAndShoot(float sqrDistance)
    {
        agent.ResetPath();
        agent.isStopped = true;

        // Rotation vers le joueur
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, lookDir, Time.deltaTime * 10f);

        HandleAttack(sqrDistance);
    }

    void Flee()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 fleePosition = transform.position - directionToPlayer * 5f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, 5f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
    }

    void HandleAttack(float sqrDistance)
    {
        //if (sqrDistance > sqrAttackRange) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 origin = transform.position + Vector3.up * enemyMovement.hauteurRayonDetection;
        Vector3 direction = (player.position + Vector3.up*0.6f - origin).normalized;
        

        // Instanciation
        GameObject proj = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(direction));
        var projectile = proj.GetComponent<Projectile>();
        projectile.Init(speed);
        projectile.OnHitEvent += (hit) =>
        {
            OnHit(hit);
        };
    }

    void OnHit(RaycastHit hit)
    {
        bool playerHit = false;

        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, hit.point);
            if (hit.collider.CompareTag("Player"))
            {
                playerHit = true;
            }
        }
        if (playerHit) 
        {
            Debug.Log("Player hit for " + damage + " damage!");
        }

    }

    void OnDrawGizmosSelected()
    {
        // Attack range (portée max)
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, attackRange);

        // Retreat distance (zone de fuite)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
    }

}
