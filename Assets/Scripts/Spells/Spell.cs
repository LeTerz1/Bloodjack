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
    public CastMode castMode;
    public float manaCost;
    public float cooldown;

    [Header("Mana Gain")]
    public bool HitRegenMana;
    public float manaRegenAmount;

    [Header("UI")]
    public GameObject damagePopupPrefab;


    public abstract void Cast(Transform castPoint, Vector3 targetPoint);
}
