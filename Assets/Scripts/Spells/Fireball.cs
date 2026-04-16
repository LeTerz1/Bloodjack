using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class Fireball : Spell
{
    public GameObject projectilePrefab;
    public float speed;
    public float damage;

    [Header("Mana Gain")]
    public bool HitRegenMana;
    public float manaRegenAmount;


    public override void Cast(Transform castPoint, Vector3 targetPoint)
    {
        Mana playerMana = castPoint.GetComponentInParent<Mana>();
        Vector3 direction = (targetPoint - castPoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, castPoint.position, Quaternion.identity);
        proj.transform.forward = direction;
        proj.transform.rotation = Quaternion.LookRotation(direction);

        var projectile = proj.GetComponent<Projectile>();
        projectile.Init(speed, damage, playerMana, HitRegenMana, manaRegenAmount);
    }
}
