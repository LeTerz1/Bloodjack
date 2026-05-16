using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Image fillImage;

    private EnemyHealth target;
    private TextMeshProUGUI enemyName;

    private void Awake()
    {
        enemyName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTarget(EnemyHealth newTarget)
    {
        target = newTarget;
        gameObject.SetActive(target != null);
        if (target == null) return;

        fillImage.fillAmount = target.GetHealthPercent();

        enemyName.text = target.enemyName;
    }
}
