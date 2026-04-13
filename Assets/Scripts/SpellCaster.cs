using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public Spell[] spellSlots = new Spell[2];
    private float[] cooldownTimers;

    public float currentMana = 100f;

    public Transform castPoint;

    void Start()
    {
        cooldownTimers = new float[spellSlots.Length];
    }

    void Update()
    {
        // Cooldowns
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
                cooldownTimers[i] -= Time.deltaTime;
        }

        // Input exemple
        if (Input.GetKeyDown(KeyCode.Mouse0))
            TryCast(0);

        if (Input.GetKeyDown(KeyCode.Mouse1))
            TryCast(1);
    }

    void TryCast(int index)
    {
        Spell spell = spellSlots[index];

        if (spell == null) return;

        if (cooldownTimers[index] > 0) return;

        if (currentMana < spell.manaCost) return;

        // Cast
        spell.Cast(castPoint);

        currentMana -= spell.manaCost;
        cooldownTimers[index] = spell.cooldown;
    }

    public void EquipSpell(int slotIndex, Spell newSpell)
    {
        spellSlots[slotIndex] = newSpell;
    }
}
