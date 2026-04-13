using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Fireball")]
public class Fireball : Spell
{
    public GameObject prefab;
    public float speed;
    public Transform point;

    public override void Cast(Transform castPoint)
    {
        //castPoint = point;

    }
}
