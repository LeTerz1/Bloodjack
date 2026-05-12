using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class Fireball : Spell
{
    private Mana playerMana;

    [Header("Projectile Stats")]
    public float speed;
    public float damage;

    [Header("Visuals")]
    public GameObject projectilePrefab;
    public GameObject hitVfx;

    public override void Cast(Transform castPoint, Vector3 targetPoint)
    {
        playerMana = castPoint.GetComponentInParent<Mana>();
        Vector3 direction = (targetPoint - castPoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, castPoint.position, Quaternion.identity);
        proj.transform.rotation = Quaternion.LookRotation(direction);

        var projectile = proj.GetComponent<Projectile>();
        projectile.Init(speed);

        // abonnement à l'event
        projectile.OnHitEvent += (hit) =>
        {
            OnHit(hit);
        };
    }

    void OnHit(RaycastHit hit)
    {
        bool enemyHit = false;

        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, hit.point);
            if (hit.collider.CompareTag("Enemy"))
            {
                enemyHit = true;
            }
        }

        Instantiate(hitVfx, hit.point, Quaternion.identity);

        if (enemyHit)
        {
            if (HitRegenMana && playerMana != null)
            {
                playerMana.RegenerateMana(manaRegenAmount);
            }
        } 
    }

}
