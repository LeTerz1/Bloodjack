using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro text;
    public float moveSpeed = 2f;
    public float lifeTime = 1f;

    private float timer;
    
    void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Setup(float damage)
    {
        text.text = damage.ToString();
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        // fade
        float alpha = 1 - (timer / lifeTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

        if (timer >= lifeTime)
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
