using UnityEngine;


public enum CastMode
{
    Single, // clic unique
    Hold    // maintien
}

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public abstract class Spell : ScriptableObject
{
    public string spellName;
    public float manaCost;
    public float cooldown;

    public CastMode castMode;

    public abstract void Cast(Transform castPoint, Vector3 targetPoint);
}
