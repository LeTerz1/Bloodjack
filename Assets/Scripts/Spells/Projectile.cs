using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;

    private Mana playerMana;
    private bool canRegenMana;
    private float manaRegenAmount;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPosition = transform.position;
    }

    public void Init(float projectileSpeed, float projectileDamage, Mana playerMana, bool canRegenMana, float manaRegenAmount)
    {
        speed = projectileSpeed;
        damage = projectileDamage;
        this.playerMana = playerMana;
        this.canRegenMana = canRegenMana;
        this.manaRegenAmount = manaRegenAmount;
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
                OnHit(hit.point);
            }

            // Dans tous les cas, on détruit à l’impact
            Destroy(gameObject);
        }

        transform.position += movement;
        previousPosition = transform.position;
    }

    void OnHit(Vector3 hitVector3)
    {
        if (canRegenMana && playerMana != null)
        {
            playerMana.RegenerateMana(manaRegenAmount);
        }
    }
}
