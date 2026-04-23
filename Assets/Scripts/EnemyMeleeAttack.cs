using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private EnemyMovement enemyMovement;

    public float attackRange = 2f;
    public float stoppingDistance = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    private float attackTimer;
    private float sqrAttackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        enemyMovement = GetComponent<EnemyMovement>();

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
        sqrAttackRange = attackRange * attackRange;
        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (player == null) return;

        if (!enemyMovement.firstDetection) return;

        float sqrDistance = (player.position - transform.position).sqrMagnitude;
        if (sqrDistance <= sqrAttackRange)
        {
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, lookDir, Time.deltaTime * 10f);
            Attack();
            return;
        }
    }

    void Attack()
    {
        // Stop le mouvement
        agent.ResetPath();

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            // Infliger dégâts
            Debug.Log("Attaque !");

            // Exemple :
            // player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
