using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPosition = transform.position;
    }

    public void Init(float projectileSpeed, float projectileDamage)
    {
        speed = projectileSpeed;
        damage = projectileDamage;
    }


    Vector3 previousPosition;
    // Update is called once per frame
    void Update()
    {
        Vector3 movement = transform.forward * speed * Time.deltaTime;

        if (Physics.Raycast(previousPosition, movement.normalized, out RaycastHit hit, movement.magnitude))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            // Dans tous les cas, on détruit à l’impact
            Destroy(gameObject);
        }

        transform.position += movement;
        previousPosition = transform.position;
    }

}
