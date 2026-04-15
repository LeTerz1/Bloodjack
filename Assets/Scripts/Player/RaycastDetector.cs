using System.Collections.Generic;
using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    private Camera playerCamera;
    private EnemyHealth currentTarget;

    public float detectionRange = 100f;
    [SerializeField] private EnemyHealthUI enemyHealthUI;

    [SerializeField] private List<LayerToRenderingLayer> layerMappings;

    private Dictionary<int, uint> layerToRenderingMask = new();

    void Awake()
    {
        playerCamera = GetComponent<SpellCaster>().playerCamera;

        // Convertit en dictionnaire pour lookup rapide
        foreach (var mapping in layerMappings)
        {
            int layer = GetLayerFromMask(mapping.physicsLayer);
            uint mask = (uint)(1 << mapping.renderingLayerIndex);

            if (!layerToRenderingMask.ContainsKey(layer))
                layerToRenderingMask.Add(layer, mask);
        }

    }

    void Update()
    {
        DetectEnemy();
    }

    void DetectEnemy()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        EnemyHealth newTarget = null;
        uint newMask = 0;

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            int hitLayer = hit.collider.gameObject.layer;

            if (layerToRenderingMask.TryGetValue(hitLayer, out uint mask))
            {
                newTarget = hit.collider.GetComponent<EnemyHealth>();
                newMask = mask;
            }
        }
        
        enemyHealthUI.SetTarget(newTarget);

        if (newTarget == currentTarget)
            return;

        

        // enlever ancien
        if (currentTarget != null)
        {
            foreach (var r in currentTarget.GetComponentsInChildren<Renderer>())
            {
                foreach (var mask in layerToRenderingMask.Values)
                {
                    r.renderingLayerMask &= ~mask;
                }
            }
        }

        // ajouter nouveau
        if (newTarget != null)
        {
            foreach (var r in newTarget.GetComponentsInChildren<Renderer>())
            {
                r.renderingLayerMask |= newMask;
            }
        }

        currentTarget = newTarget;
    }

    //  récupère le layer depuis un LayerMask (1 seul layer attendu)
    int GetLayerFromMask(LayerMask mask)
    {
        int value = mask.value;
        for (int i = 0; i < 32; i++)
        {
            if ((value & (1 << i)) != 0)
                return i;
        }
        return 0;
    }
}
