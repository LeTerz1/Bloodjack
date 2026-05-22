using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    public bool launchAtStart = true;

    public event Action<RaycastHit> OnHitEvent;

    Vector3 previousPosition;

    public void Init(float projectileSpeed)
    {
        speed = projectileSpeed;
    }

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        if (launchAtStart == false)
            return;
        Vector3 movement = transform.forward * speed * Time.deltaTime;

        if (Physics.Raycast(previousPosition, movement.normalized, out RaycastHit hit, movement.magnitude))
        {
            // on notifie
            OnHitEvent?.Invoke(hit);

            Destroy(gameObject);
        }

        transform.position += movement;
        previousPosition = transform.position;
    }
}
