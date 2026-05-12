using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class ConeSpellBehaviour : MonoBehaviour
{
    public ConeSpell coneSpell;
    private Vector3 debugChainPosition;
    private bool hasDebugChain;

    void OnDrawGizmosSelected()
    {
        if (coneSpell == null) return;

        // Cone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coneSpell.range);

        Vector3 left = Quaternion.Euler(0, -coneSpell.angle, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, coneSpell.angle, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, left * coneSpell.range);
        Gizmos.DrawRay(transform.position, right * coneSpell.range);

        //Chain radius
        if (hasDebugChain)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(debugChainPosition, coneSpell.chainRadius);
        }
    }

    public void ResetChainDebug()
    {
        hasDebugChain = false;
    }

    public void StartChain(Transform firstTarget)
    {
        var hitTargets = new HashSet<Transform>();
        hitTargets.Add(firstTarget);

        Collider col = firstTarget.GetComponent<Collider>();
        Vector3 startPosition = col != null ? col.bounds.center : firstTarget.position;

        StartCoroutine(ChainDamage(startPosition, 0, hitTargets));
    }

    public IEnumerator ChainDamage(Vector3 lastHitPosition, int chainIndex, HashSet<Transform> hitTargets)
    {
        if (chainIndex >= coneSpell.maxChains)
        {
            yield break;
        }

        // Debug visuel
        debugChainPosition = lastHitPosition;
        hasDebugChain = true;

        yield return new WaitForSeconds(coneSpell.chainDelay);

        Collider[] hits = Physics.OverlapSphere(lastHitPosition, coneSpell.chainRadius, LayerMask.GetMask("Enemy"));

        Transform nextTarget = null;
        float bestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hitTargets.Contains(hit.transform))
                continue;

            float dist = Vector3.Distance(lastHitPosition, hit.transform.position);

            if (dist < bestDistance)
            {
                bestDistance = dist;
                nextTarget = hit.transform;
            }
        }

        if (nextTarget != null)
        {
            hitTargets.Add(nextTarget);

            Collider nextCol = nextTarget.GetComponent<Collider>();
            Vector3 nextPosition = nextCol != null ? nextCol.bounds.center : nextTarget.position;

            SpawnLine(lastHitPosition, nextTarget);
            Instantiate(coneSpell.hitEffectPrefab, nextPosition, Quaternion.identity);

            IDamageable dmg = nextTarget.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(coneSpell.secondaryDamage, nextPosition);
            }

            yield return ChainDamage(nextPosition, chainIndex + 1, hitTargets);
        }
    }

    void SpawnLine(Vector3 start, Transform target)
    {
        LineRenderer line = Instantiate(coneSpell.linePrefab);

        line.positionCount = 2;

        line.SetPosition(0, start);

        // centre collider
        Collider col = target.GetComponent<Collider>();
        Vector3 targetPos = target.position;

        if (col != null)
            targetPos = col.bounds.center;

        line.SetPosition(1, targetPos);

        Destroy(line.gameObject, coneSpell.lineDuration);
    }

}
