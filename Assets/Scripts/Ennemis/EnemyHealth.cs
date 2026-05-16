using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    private float currentHealth;
    public string enemyName;
    public GameObject damagePopupPrefab;
    private Coroutine flashRoutine;

    [Header("Hit Feedback")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float flashDuration = 0.1f;

    private Renderer[] renderers;
    private Material[][] originalMaterials;

    void Start()
    {
        currentHealth = maxHealth;
        // Récupère TOUS les renderers dans les enfants
        renderers = GetComponentsInChildren<Renderer>();

        // Stocke les matériaux d'origine
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }

    }

    void ShowDamage(Vector3 worldPosition, float damageAmount)
    {
        if (damagePopupPrefab == null) return;
        GameObject popup = Instantiate(damagePopupPrefab, worldPosition, Quaternion.identity);
        popup.GetComponent<DamagePopup>().Setup(damageAmount);
    }

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        currentHealth -= amount;
        ShowDamage(hitPoint, amount);
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(HitFlash());


        Debug.Log("PV restants : " + currentHealth);
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.firstDetection = true;
            enemyMovement.AlertNearbyEnemies();
        }


        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Debug.Log("Enemy mort");
        Destroy(gameObject);
    }

    IEnumerator HitFlash()
    {
        // Applique le material de hit à tous les renderers
        foreach (var r in renderers)
        {
            Material[] hitMats = new Material[r.materials.Length];

            for (int i = 0; i < hitMats.Length; i++)
                hitMats[i] = hitMaterial;

            r.materials = hitMats;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restaure les matériaux d'origine
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
