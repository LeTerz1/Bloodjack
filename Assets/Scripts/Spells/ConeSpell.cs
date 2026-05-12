using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/ConeSpell")]
public class ConeSpell : Spell
{
    [Header("Damages")]
    public float damage = 20f;
    public float secondaryDamage = 10f;

    [Header("Detection")]
    public float range = 10f;
    public float angle = 45f;

    [Header("Chain Settings")]
    public float chainRadius = 5f;
    public float chainDelay = 0.2f;
    public int maxChains = 3;

    [Header("VFX")]
    public LineRenderer linePrefab;
    public float lineDuration = 0.2f;
    public GameObject hitEffectPrefab;
    public GameObject electricSpray;


    public override void Cast(Transform castPoint, Vector3 targetPoint)
    {
        Transform bestTarget = FindClosestTargetInCone(castPoint);

        GameObject fx = Instantiate(electricSpray, castPoint.position, castPoint.rotation);
        fx.transform.SetParent(castPoint);
        Destroy(fx, 3f);

        if (bestTarget != null)
        {
            

            // centre du collider
            Collider col = bestTarget.GetComponent<Collider>();
            Vector3 targetPosition = bestTarget.position;

            if (col != null)
            {
                targetPosition = col.bounds.center;
            }


            Instantiate(hitEffectPrefab, targetPosition, Quaternion.identity);

            IDamageable dmg = bestTarget.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage, targetPosition);

                ConeSpellBehaviour coneSpellBehaviour = castPoint.GetComponentInParent<ConeSpellBehaviour>();

                var hitTargets = new HashSet<Transform>();
                hitTargets.Add(bestTarget);

                coneSpellBehaviour.StartChain(bestTarget);
            }
        }
    }

    Transform FindClosestTargetInCone(Transform origin)
    {
        Collider[] hits = Physics.OverlapSphere(origin.position, range, LayerMask.GetMask("Enemy"));

        Transform bestTarget = null;
        float bestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - origin.position).normalized;

            float angleToTarget = Vector3.Angle(origin.forward, dirToTarget);

            if (angleToTarget <= angle)
            {
                float distance = Vector3.Distance(origin.position, hit.transform.position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestTarget = hit.transform;
                }
            }
        }

        return bestTarget;
    }
}
