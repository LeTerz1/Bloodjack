using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    public float updateRate = 0.2f; // Temps entre chaque update de destination
    private float timer;

    [Header("Ranges")]
    public float detectionRange = 10f;
    public float visionRange = 20f;
    [Range(0, 360)]
    public float visionAngle = 90f;
    public float hauteurRayonDetection = 1.5f; // Hauteur à laquelle le rayon de détection est lancé
    public LayerMask visionMask;

    private float sqrDetectionRange;
    private float sqrVisionRange;

    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
        sqrDetectionRange = detectionRange * detectionRange; // Utiliser la distance au carré pour éviter les calculs de racine carrée
        sqrVisionRange = visionRange * visionRange;
    }

    public bool firstDetection = false;
    void Update()
    {
        if (player == null) return;

        // Si pas encore détecté + on check
        if (!firstDetection)
        {
            if (DetectionPlayer())
            {
                firstDetection = true;
                AlertNearbyEnemies();
            }
            else
            {
                return;
            }
        }

        // Timer pour limiter SetDestination
        timer += Time.deltaTime;

        if (timer >= updateRate)
        {
            agent.SetDestination(player.position);
            timer = 0f;
        }
    }

    bool DetectionPlayer()
    {
        Vector3 directionToPlayer = player.position+Vector3.up*0.6f - transform.position;
        float sqrDistance = directionToPlayer.sqrMagnitude;

        bool inDetectionRange = sqrDistance <= sqrDetectionRange;
        bool inVisionRange = sqrDistance <= sqrVisionRange;

        // Cercle (traverse les murs)
        if (inDetectionRange)
            return true;

        // Vision (avec FOV + Raycast)
        if (inVisionRange && IsInFOV(directionToPlayer) && HasLineOfSight(directionToPlayer))
            return true;

        return false;
    }
    bool IsInFOV(Vector3 directionToPlayer)
    {
        directionToPlayer.y = 0f; // ignore hauteur si besoin

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle <= visionAngle * 0.5f;
    }
    public bool HasLineOfSight(Vector3 directionToPlayer)
    {
        Vector3 origin = transform.position + Vector3.up * hauteurRayonDetection;

        //recalcul correct de la direction
        Vector3 target = player.GetComponent<Collider>().bounds.center;
        Vector3 direction = (target - origin).normalized;

        Debug.DrawRay(origin, direction * visionRange, Color.green);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, visionRange, visionMask))
        {
            return hit.transform == player;
        }

        return false;
    }

    public void AlertNearbyEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Enemy"));

        foreach (Collider hit in hits)
        {
            EnemyMovement enemy = hit.GetComponent<EnemyMovement>();

            if (enemy != null && !enemy.firstDetection)
            {
                enemy.firstDetection = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vision range (cône approx)
        Gizmos.color = Color.yellow;

        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, leftBoundary * visionRange);
        Gizmos.DrawRay(transform.position, rightBoundary * visionRange);

        Gizmos.DrawIcon(transform.position + Vector3.up * hauteurRayonDetection, "eye_icon.png", true);
    }
}
