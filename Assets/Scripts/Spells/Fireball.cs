using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class Fireball : Spell
{
    public GameObject projectilePrefab;
    public float speed;
    public float damage;

    public override void Cast(Transform castPoint)
    {
        GameObject proj = Instantiate(projectilePrefab, castPoint.position, castPoint.rotation);

        var projectile = proj.GetComponent<Projectile>();
        projectile.Init(speed, damage);
    }
}
