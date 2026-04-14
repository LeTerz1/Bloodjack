using UnityEngine;

public class TargetHighlight : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock mpb;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void SetHighlight(bool state)
    {
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Highlight", state ? 1f : 0f);
        rend.SetPropertyBlock(mpb);
    }

   
}
