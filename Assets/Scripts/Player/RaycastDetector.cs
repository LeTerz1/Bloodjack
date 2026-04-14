using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    private Camera playerCamera;
    private EnemyHealth currentTarget;
    [SerializeField] private LayerMask targetMask;

    void Awake()
    {
        playerCamera = GetComponent<SpellCaster>().playerCamera;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
    }

    void DetectEnemy()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        EnemyHealth newTarget = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetMask))
        {
            newTarget = hit.collider.GetComponent<EnemyHealth>();
        }

        // rien changé = on sort
        if (newTarget == currentTarget)
            return;

        // désactiver ancien
        if (currentTarget != null)
        {
            currentTarget.outlineMaterial.enabled = false;
        }

        // activer nouveau
        if (newTarget != null)
        {
            newTarget.outlineMaterial.enabled = true;
        }

        currentTarget = newTarget;
    }
}
