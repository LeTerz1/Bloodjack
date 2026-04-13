using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public abstract class Spell : ScriptableObject
{
    public string spellName;
    public float manaCost;
    public float cooldown;

    public abstract void Cast(Transform castPoint);
}
