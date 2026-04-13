using UnityEngine;
using UnityEngine.InputSystem;

public class CastSpell : MonoBehaviour
{
    public GameObject[] spells;
    public Transform firePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.qKey.isPressed)
        {
            Debug.Log("Casting spell!");
            LaunchSpell(0);

        }
    }

    public void LaunchSpell(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= spells.Length)
        {
            Debug.LogError("Invalid spell index: " + spellIndex);
            return;
        }
        Instantiate(spells[spellIndex], firePoint.position, firePoint.rotation);
    }
}
