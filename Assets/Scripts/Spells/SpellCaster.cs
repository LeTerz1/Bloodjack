using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellCaster : MonoBehaviour
{
    public Spell[] spellSlots = new Spell[2];
    private float[] cooldownTimers;
    private bool[] isHolding;

    private Mana mana;

    //private float currentMana = 100f;

    public Transform castPoint;
    public Camera playerCamera;

    public Image[] cooldownImages;

    private PlayerInputActions controls;

    void Awake()
    {
        controls = new PlayerInputActions();

        controls.Player.Spell0.started += ctx => StartCasting(0);
        controls.Player.Spell0.canceled += ctx => StopCasting(0);

        controls.Player.Spell1.started += ctx => StartCasting(1);
        controls.Player.Spell1.canceled += ctx => StopCasting(1);

        mana = GetComponent<Mana>();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        cooldownTimers = new float[spellSlots.Length];
        isHolding = new bool[spellSlots.Length];
    }

    void Update()
    {
        // Cooldowns
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
                cooldownTimers[i] -= Time.deltaTime;
        }

        // Casting continu
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (isHolding[i])
            {
                TryCast(i);
            }
        }

        // Update cooldown UI
        for (int i = 0; i < cooldownImages.Length; i++)
        {
            if (i < cooldownTimers.Length)
            {
                cooldownImages[i].fillAmount = 0 + (cooldownTimers[i] / spellSlots[i].cooldown);
            }
        }
    }

    public Vector3 GetAimPoint()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            return hit.point;

        return ray.origin + ray.direction * 100f;
    }

    void TryCast(int index)
    {
        Spell spell = spellSlots[index];
        if (spell == null) return;

        if (cooldownTimers[index] > 0)
        {
            Debug.Log($"Spell {spell.spellName} is on cooldown for {cooldownTimers[index]:F1} more seconds.");
            return;
        }
            

        if (mana.current < spell.manaCost)
        {
            Debug.Log($"Not enough mana to cast {spell.spellName}. Current mana: {mana.current}");
            return;
        }

        Vector3 target = GetAimPoint();
        spell.Cast(castPoint, target);

        mana.ConsumeMana(spell.manaCost);
        cooldownTimers[index] = spell.cooldown;
    }

    void StartCasting(int index)
    {
        Spell spell = spellSlots[index];
        if (spell == null) return;

        switch (spell.castMode)
        {
            case CastMode.Single:
                TryCast(index);
                break;

            case CastMode.Hold:
                isHolding[index] = true;
                break;
        }
    }

    void StopCasting(int index)
    {
        isHolding[index] = false;
    }

    public void EquipSpell(int slotIndex, Spell newSpell)
    {
        spellSlots[slotIndex] = newSpell;
    }
}
