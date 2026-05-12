using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float max = 100;
    private float current;
    public Image fillImage;

    [Header("Damage Feedback")]
    public RawImage bloodScreen;
    public float alphaBlood = 0.3f;
    public float fadeDuration = 0.5f;
    private Coroutine damageScreenCoroutine;

    [Header("Regeneration")]
    public float regenDelay = 5f;        // temps sans dégâts avant regen
    public float regenAmount = 10f;        // HP par seconde
    private float lastDamageTime;


    void Start()
    {
        current = max;
        UIupdate();
    }

    void Update()
    {
        HandleRegen();
    }

    void UIupdate()
    {
        fillImage.fillAmount = current / max;
    }

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        current -= amount;
        current = Mathf.Clamp(current, 0, max);

        lastDamageTime = Time.time; // reset timer regen

        TriggerBloodEffect();
        UIupdate();

        if (current <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        Debug.Log("Player mort");
        // Implémentez la logique de mort du joueur ici (ex: recharger la scène, afficher un écran de game over, etc.)
    }

    void HandleRegen()
    {
        // si assez de temps sans dégâts
        if (Time.time - lastDamageTime >= regenDelay && current < max)
        {
            current += regenAmount * Time.deltaTime;
            current = Mathf.Clamp(current, 0, max);

            UIupdate();
        }
    }

    void TriggerBloodEffect()
    {
        if (damageScreenCoroutine != null)
            StopCoroutine(damageScreenCoroutine);

        damageScreenCoroutine = StartCoroutine(BloodFlash());
    }
    IEnumerator BloodFlash()
    {
        // 1. flash immédiat
        SetAlpha(alphaBlood);

        float t = 0f;

        // 2. fade vers 0
        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Lerp(alphaBlood, 0f, t / fadeDuration);
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(0f);
    }
    void SetAlpha(float a)
    {
        Color c = bloodScreen.color;
        c.a = a;
        bloodScreen.color = c;
    }
}
