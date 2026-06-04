using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossProjectiles : MonoBehaviour
{
    private Transform player;
    public Transform projectilesAnchor;
    private float timer;
    [Header("Projectile")]
    public GameObject projectilePrefab;
    private List<Projectile> spawnedProjectiles = new();
    public float projectileSpeed = 20f;
    public int damage = 10;

    [Header("Spawn Points")]
    public Transform[] projectilesPos;

    [Header("Launch Settings")]
    public float delayBeforeLaunch = 1f;
    public float delayBetweenShots = 0.5f;
    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        projectilesAnchor.transform.LookAt(player.position);
    }

    public void SpawnProjectiles()
    {
        spawnedProjectiles.Clear();

        foreach (Transform spawnPoint in projectilesPos)
        {
            if (spawnPoint == null)
                continue;

            GameObject obj = Instantiate(projectilePrefab,spawnPoint.position,spawnPoint.rotation,spawnPoint);

            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.launchAtStart = false;
            spawnedProjectiles.Add(projectile);
        }

        StartCoroutine(LaunchProjectiles1by1());
    }

    IEnumerator LaunchProjectiles1by1()
    {
        yield return new WaitForSeconds(delayBeforeLaunch);

        foreach (Projectile projectile in spawnedProjectiles)
        {
            projectile.launchAtStart = true;
            projectile.Init(projectileSpeed);
            Vector3 direction = (player.position + Vector3.up * 0.3f - projectile.transform.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.OnHitEvent += (hit) =>
            {
                OnHit(hit);
            };

            yield return new WaitForSeconds(delayBetweenShots);
        }
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
}
