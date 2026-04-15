using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public float max = 100f;
    public float current = 100f;
    public Image fillImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current = max;
    }

    // Update is called once per frame
    void Update()
    {
        UIupdate();
    }

    void UIupdate()
    {
        fillImage.fillAmount = current / max;
    }
}
