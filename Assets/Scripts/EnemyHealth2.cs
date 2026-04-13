using UnityEngine;

public class EnemyHealth2 : MonoBehaviour //,IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log("PV restants : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy mort");
        Destroy(gameObject);
    }
}
