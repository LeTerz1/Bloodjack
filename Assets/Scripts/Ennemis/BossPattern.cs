using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossPattern : MonoBehaviour
{
    public BossState currentState;
    private NavMeshAgent agent;
    [Header("Summon")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int enemiesToSpawn = 3;
    [SerializeField] private float spawnRadius = 2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (Checkpoint.bossFightStarted)
        {
            ChangeState(BossState.Move);
        }
    }

    private void Update()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
    }

    public void ChangeState(BossState newState)
    {
        currentState = newState;
        StopMove();

        switch (currentState)
        {
            case BossState.Idle:
                break;

            case BossState.Move:
                Random.Range(0, 2); // 0 ou 1
                if (Random.Range(0, 2) == 0)
                    MoveToPlayer();
                else MoveRandom();
                StartCoroutine(ChangeStateDelay(3f)); // change d'état après 5 secondes
                break;

            case BossState.Projectiles:
                GetComponent<BossProjectiles>().SpawnProjectiles();
                StartCoroutine(ChangeStateDelay(2f));
                break;

            case BossState.Summon:
                SummonEnemies();
                StartCoroutine(ChangeStateDelay(3f));
                break;
        }
    }
    private void StopMove()
    {
        agent.ResetPath();
    }

    private void SummonEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject prefab =
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            Vector3 randomOffset =
                Random.insideUnitSphere * spawnRadius;

            randomOffset.y = 0;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                    transform.position + randomOffset,
                    out hit,
                    spawnRadius,
                    NavMesh.AllAreas))
            {
                Instantiate(prefab, hit.position, Quaternion.identity);
            }
        }
    }

    public void MoveRandom()
    {
        float radius = 10f; // distance max autour du boss

        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void MoveToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    IEnumerator ChangeStateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        switch (currentState)
        {
            case BossState.Move:
                ChangeState(BossState.Projectiles);
                break;

            case BossState.Projectiles:
                ChangeState(BossState.Summon);
                break;

            case BossState.Summon:
                ChangeState(BossState.Move);
                break;
        }
    }


}

public enum BossState
{
    Idle,
    Move,
    Projectiles,
    Summon
}



